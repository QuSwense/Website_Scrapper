using HtmlAgilityPack;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using SqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using WebCommon.Error;
using WebCommon.PathHelp;
using WebReader.Xml;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The main class for parsing the scrap xml configuration file
    /// and do the scrapping
    /// </summary>
    public class WebScrapConfigParser : IScrapParser
    {
        /// <summary>
        /// Reference to the parent Web context object
        /// </summary>
        public IScrapHtmlContext WebContext { get; protected set; }

        /// <summary>
        /// The web context
        /// </summary>
        public IScrapDbContext WebDbContext
        {
            get { return WebContext.EngineContext.WebDbContext; }
        }

        /// <summary>
        /// The web context
        /// </summary>
        public AppTopicConfigPathHelper AppTopicPath
        {
            get { return WebContext.EngineContext.AppTopicPath; }
        }

        /// <summary>
        /// A xml configuration reader
        /// </summary>
        public DXmlDocReader XmlConfigReader { get; protected set; }

        /// <summary>
        /// The Html helper command class
        /// </summary>
        public HtmlScrapperCommand ScrapperCommand { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebScrapConfigParser() { }

        /// <summary>
        /// Constructor parameterized
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(WebScrapHtmlContext context)
        {
            WebContext = context;

            // Initialize the data store
            ScrapperCommand = new HtmlScrapperCommand();
        }

        /// <summary>
        /// The method to call by the implementor.
        /// This method starts the scrapping of data
        /// </summary>
        public void Run()
        {
            // Load the scrap xml config
            AppTopicPath.AppTopicScrap.AssertExists();
            XmlConfigReader = DXmlDocReader.Create(AppTopicPath.AppTopicScrap.FullPath);

            // Read the Webdata node which is the start node of any app topic
            foreach (XmlNode rootWebdataNode in XmlConfigReader.ReadNodes("//WebData"))
            {
                ParseChildScrapNodes(new WebScrapParserStateModel()
                {
                    CurrentXmlNode = rootWebdataNode
                });
            }
        }

        /// <summary>
        /// This is the parse method to parse the "Webdata" element tag
        /// </summary>
        /// <param name="rootScrapNode"></param>
        public void ParseChildScrapNodes(WebScrapParserStateModel state)
        {
            // Read the child nodes of Scrap type nodes
            foreach (XmlNode rootScrapTypeNode in state.CurrentXmlNode.ChildNodes)
            {
                ConfigScrapElementFactory(new WebScrapParserStateModel()
                {
                     ConfigScrap = state.ConfigScrap,
                     CurrentHtmlNode = state.CurrentHtmlNode,
                     CurrentXmlNode = rootScrapTypeNode
                });
            }
        }

        /// <summary>
        /// This is the start of any Scrap type node. A scrap type node is a single entity
        /// which contains all information to scrap a website page for information, extract data,
        /// and save into table
        /// </summary>
        /// <param name="scrapTypeNode"></param>
        private void ConfigScrapElementFactory(WebScrapParserStateModel state)
        {
            if (state == null || state.CurrentXmlNode == null) return;
            if (state.CurrentXmlNode.LocalName == "ScrapHtmlTable")
                ParseScrapHtmlTableElement(state);
        }

        /// <summary>
        /// Parse The 'ScrapHtmlTable' element tag
        /// </summary>
        /// <param name="rootScrapTypeNode"></param>
        private void ParseScrapHtmlTableElement(WebScrapParserStateModel state)
        {
            // Construct the scrap element state
            ScrapHtmlTableConfigParser scrapHtmlTableParser = new ScrapHtmlTableConfigParser(this, state);
            scrapHtmlTableParser.Run();
        }

        

        /// <summary>
        /// Parse Root Scrap Element
        /// </summary>
        /// <param name="reader"></param>
        private void ParseScrapElement(XmlNode scrapNode, WebDataConfigScrapHtmlTable parentScrapConfigObj = null,
            HtmlNodeNavigator webNodeNavigator = null)
        {
            // Construct the scrap element state
            WebDataConfigScrapHtmlTable webScrapConfigObj = ParseScrapElementAttributes(scrapNode, parentScrapConfigObj, webNodeNavigator);

            // If the scrap element type is TABLE type that means
            // The child data needs to be processed in a loop
            if (webScrapConfigObj.Type == EWebDataConfigType.TABLE)
            {
                // This finally scraps the html webpage data
                List<HtmlNodeNavigator> webChildNodeNavigators = ProcessHtmlTableScrapElement(webScrapConfigObj);
                ProcessAsHtmlTableType(webScrapConfigObj, scrapNode, webChildNodeNavigators);
            }
            else if(webScrapConfigObj.Type == EWebDataConfigType.CSV)
            {
                using(StringReader stringReader = new StringReader(ProcessCSVTableScrapElement(webScrapConfigObj)))
                    ProcessAsCsvType(webScrapConfigObj, scrapNode, stringReader);
            }
        }

        private void ProcessAsCsvType(WebDataConfigScrapHtmlTable webScrapConfigObj, XmlNode scrapNode, StringReader txtReader)
        {
            int nodeIndex = -1;
            string fileLine = "";
            while ((fileLine = txtReader.ReadLine()) != null)
            {
                nodeIndex++;
                if (webScrapConfigObj.SkipFirstLines > nodeIndex) continue;

                // Read the Column nodes which are the individual reader config nodes
                ProcessColumnNodes(nodeIndex, webScrapConfigObj, scrapNode, fileLine);

                nodeIndex++;
            }
        }

        private void ProcessColumnNodes(int nodeIndex, WebDataConfigScrapHtmlTable webScrapConfigObj, XmlNode scrapNode, string fileLine)
        {
            if (webScrapConfigObj.Columns.Count <= 0)
            {
                XmlNodeList columnNodeList = scrapNode.SelectNodes("Column");

                if (columnNodeList != null && columnNodeList.Count > 0)
                {
                    foreach (XmlNode columnNode in columnNodeList)
                    {
                        webScrapConfigObj.Columns.Add(
                            ParseColumnElement(nodeIndex, columnNode, webScrapConfigObj));
                    }
                }

                // Load the table with partial columns in memory
                WebDbContext.AddMetadata(webScrapConfigObj);
            }

            ColumnScrapIterator(nodeIndex, webScrapConfigObj, scrapNode, fileLine);
        }

        private void ColumnScrapIterator(int nodeIndex, WebDataConfigScrapHtmlTable scrapConfig, XmlNode scrapNode, string fileLine)
        {
            List<DynamicTableDataInsertModel> row = new List<DynamicTableDataInsertModel>();
            bool doSkipUpdate = false;
            string[] splitData = fileLine.Split(new char[] { scrapConfig.Delimiter[0] });
            for (int indx = 0; indx < scrapConfig.Columns.Count; ++indx)
            {
                WebDataConfigColumn columnConfig = scrapConfig.Columns[indx];
                string scrappedData = splitData[columnConfig.Index];
                ManipulateHtmlData manipulateHtml = Manipulate(columnConfig, columnConfig.Manipulations, scrappedData);

                DynamicTableDataInsertModel tableDataColumn = new DynamicTableDataInsertModel();

                if (columnConfig.IsUnique && string.IsNullOrEmpty(manipulateHtml.Value))
                {
                    doSkipUpdate = true;
                    break;
                }

                row.Add(tableDataColumn);

                tableDataColumn.Name = columnConfig.Name;
                tableDataColumn.IsPk = columnConfig.IsUnique;
                tableDataColumn.RowIndex = nodeIndex;
                tableDataColumn.Value = manipulateHtml.Value;
                tableDataColumn.DataType =
                    WebDbContext.MetaDbConfig.TableColumnConfigs[scrapConfig.Name][columnConfig.Name].DataType;
            }

            if (!doSkipUpdate)
                WebContext.EngineContext.WebDbContext.AddOrUpdate(scrapConfig, row);
        }

        private ManipulateHtmlData Manipulate(WebDataConfigColumn columnConfig, List<WebDataConfigManipulate> manipulateNodeList, string scrappedData)
        {
            ManipulateHtmlData result = new ManipulateHtmlData();

            string data = scrappedData;
            if (manipulateNodeList != null && manipulateNodeList.Count > 0)
            {
                result.Value = data;
                foreach (WebDataConfigManipulate manipulate in manipulateNodeList)
                {
                    if (!string.IsNullOrEmpty(result.Value))
                    {
                        if (manipulate.Trims != null)
                        {
                            List<char> trimChars = new List<char>();
                            foreach (var trimNode in manipulate.Trims)
                            {
                                trimChars.Add(trimNode.Data[0]);
                            }

                            result.Value = result.Value.Trim(trimChars.ToArray());
                        }
                    }
                    if (!string.IsNullOrEmpty(result.Value))
                    {
                        if (manipulate.Splits != null)
                        {
                            foreach (WebDataConfigSplit splitConfig in manipulate.Splits)
                            {
                                string[] split = result.Value.Split(splitConfig.Data.ToArray());
                                result.Value = split[splitConfig.Index];
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(result.Value))
                    {
                        if (manipulate.Replaces != null)
                        {
                            foreach (WebDataConfigReplace replaceConfig in manipulate.Replaces)
                            {
                                result.Value = result.Value.Replace(replaceConfig.InString, replaceConfig.OutString);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(result.Value))
                    {
                        if (manipulate.Regexes != null)
                        {
                            foreach (WebDataConfigRegex regexConfig in manipulate.Regexes)
                            {
                                Match output = Regex.Match(result.Value,
                                    regexConfig.Pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                                result.Value = output.Groups[regexConfig.Index].Value;
                            }
                        }
                    }
                }
            }
            else
            {
                result.Value = data;
            }

            result.XPath = columnConfig.XPath;
            result.Value = HtmlEntity.DeEntitize(result.Value);

            return result;
        }

        private string ProcessCSVTableScrapElement(WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            return ScrapperCommand.LoadFile(webScrapConfigObj.Url);
        }

        

        /// <summary>
        /// Process the parsed Scrap element tag
        /// This method uses the xml config Scrap information to get to the Html webpage path
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        private List<HtmlNodeNavigator> ProcessHtmlTableScrapElement(WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            HtmlNode htmlDoc = ScrapperCommand.Load(webScrapConfigObj.Url);
            return ScrapperCommand.ReadNodes(htmlDoc, webScrapConfigObj.XPath);
        }

        /// <summary>
        /// Process the Scrap element data as HTML Table type
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="scrapNode"></param>
        private void ProcessAsHtmlTableType(WebDataConfigScrapHtmlTable webScrapConfigObj, XmlNode scrapNode,
            List<HtmlNodeNavigator> webNodeNavigatorList)
        {
            int nodeIndex = 0;
            foreach (var webNodeNavigator in webNodeNavigatorList)
            {
                // Read the child Scraps nodes which are the individual reader config nodes
                ProcessChildScrapNodes(webScrapConfigObj, scrapNode, webNodeNavigator);

                // Read the Column nodes which are the individual reader config nodes
                ProcessColumnNodes(nodeIndex, webScrapConfigObj, scrapNode, webNodeNavigator);

                nodeIndex++;
            }
        }

        /// <summary>
        /// Parse and Process any Child Scrap Nodes (recursively)
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="scrapNode"></param>
        /// <param name="webNodeNavigator"></param>
        private void ProcessChildScrapNodes(WebDataConfigScrapHtmlTable webScrapConfigObj, XmlNode scrapNode,
            HtmlNodeNavigator webNodeNavigator)
        {
            XmlNodeList childScrapNodeList = scrapNode.SelectNodes("Scrap");

            if (childScrapNodeList != null && childScrapNodeList.Count > 0)
            {
                foreach (XmlNode childScrapNode in childScrapNodeList)
                {
                    ParseScrapElement(childScrapNode, webScrapConfigObj, webNodeNavigator);
                }
            }
        }

        /// <summary>
        /// Parse and process column nodes
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="scrapNode"></param>
        /// <param name="webNodeNavigator"></param>
        private void ProcessColumnNodes(int nodeIndex, WebDataConfigScrapHtmlTable webScrapConfigObj, XmlNode scrapNode,
            HtmlNodeNavigator webNodeNavigator)
        {
            // Check the constraints on the Scrap nodes
            // 1. Only maximum 4 levels is allowed
            // 2. Only one "name" tag should be present from the top level to bottom Scrap
            //    If multiple "name" tag is present throw error
            CheckMaxLevelConstraint(webScrapConfigObj);
            CheckScrapNameAttribute(webScrapConfigObj);

            if (webScrapConfigObj.Columns.Count <= 0)
            {
                XmlNodeList columnNodeList = scrapNode.SelectNodes("Column");

                if (columnNodeList != null && columnNodeList.Count > 0)
                {
                    foreach (XmlNode columnNode in columnNodeList)
                    {
                        webScrapConfigObj.Columns.Add(
                            ParseColumnElement(nodeIndex, columnNode, webScrapConfigObj));
                    }
                }

                // Load the table with partial columns in memory
                WebDbContext.AddMetadata(webScrapConfigObj);
            }

            ColumnScrapIterator(nodeIndex, webScrapConfigObj, scrapNode, webNodeNavigator);
        }

        /// <summary>
        /// Parse the Column tag in config file
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private WebDataConfigColumn ParseColumnElement(int nodeIndex, XmlNode columnNode, WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            WebDataConfigColumn webColumnConfigObj = ParseColumnElementAttributes(columnNode, webScrapConfigObj);

            // Check if manipulate tag is present
            ParseManipulateElement(columnNode, webColumnConfigObj);

            return webColumnConfigObj;
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseManipulateElement(XmlNode columnNode, WebDataConfigColumn webColumnConfigObj)
        {
            XmlNodeList manipulateNodeList = columnNode.SelectNodes("Manipulate");

            if (manipulateNodeList != null && manipulateNodeList.Count > 0)
            {
                foreach (XmlNode manipulateNode in manipulateNodeList)
                {
                    WebDataConfigManipulate webManipulateConfigObj = new WebDataConfigManipulate();
                    ParseManipulateChildElement(manipulateNode, webManipulateConfigObj);
                    webColumnConfigObj.Manipulations.Add(webManipulateConfigObj);
                }
            }
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseManipulateChildElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            ParseSplitElement(manipulateNode, webManipulateConfigObj);
            ParseTrimElement(manipulateNode, webManipulateConfigObj);
            ParseReplaceElement(manipulateNode, webManipulateConfigObj);
            ParseRegexElement(manipulateNode, webManipulateConfigObj);
        }

        private void ParseRegexElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            XmlNodeList splitNodeList = manipulateNode.SelectNodes("Regex");

            if (splitNodeList != null && splitNodeList.Count > 0)
            {
                foreach (XmlNode splitNode in splitNodeList)
                {
                    WebDataConfigRegex configRegexObj = XmlConfigReader.ReadElement<WebDataConfigRegex>(splitNode);
                    configRegexObj.Pattern = HttpUtility.HtmlDecode(configRegexObj.Pattern);
                    webManipulateConfigObj.Regexes.Add(configRegexObj);
                }
            }
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseSplitElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            XmlNodeList splitNodeList = manipulateNode.SelectNodes("Split");

            if (splitNodeList != null && splitNodeList.Count > 0)
            {
                foreach (XmlNode splitNode in splitNodeList)
                {
                    WebDataConfigSplit configSpliObj = XmlConfigReader.ReadElement<WebDataConfigSplit>(splitNode);
                    webManipulateConfigObj.Splits.Add(configSpliObj);
                }
            }
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseTrimElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            XmlNodeList trimNodeList = manipulateNode.SelectNodes("Trim");

            if (trimNodeList != null && trimNodeList.Count > 0)
            {
                foreach (XmlNode trimNode in trimNodeList)
                {
                    WebDataConfigTrim configTrimObj = XmlConfigReader.ReadElement<WebDataConfigTrim>(trimNode);
                    webManipulateConfigObj.Trims.Add(configTrimObj);
                    configTrimObj.Data = Normalize(configTrimObj.Data);
                }
            }
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseReplaceElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            XmlNodeList replaceNodeList = manipulateNode.SelectNodes("Replace");

            if (replaceNodeList != null && replaceNodeList.Count > 0)
            {
                foreach (XmlNode replaceNode in replaceNodeList)
                {
                    WebDataConfigReplace configTrimObj = XmlConfigReader.ReadElement<WebDataConfigReplace>(replaceNode);
                    webManipulateConfigObj.Replaces.Add(configTrimObj);
                    configTrimObj.InString = Normalize(configTrimObj.InString);
                    configTrimObj.OutString = Normalize(configTrimObj.OutString);
                }
            }
        }

        private string Normalize(string data)
        {
            if (string.IsNullOrEmpty(data)) return data;
            return data.Replace("\\n", "\n").Replace("\\t", "\t");
        }

        /// <summary>
        /// Parse the Config Xml Column attribute
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private WebDataConfigColumn ParseColumnElementAttributes(XmlNode columnNode, WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            WebDataConfigColumn webColumnConfigObj = XmlConfigReader.ReadElement<WebDataConfigColumn>(columnNode);
            webColumnConfigObj.Parent = webScrapConfigObj;
            webColumnConfigObj.Name = webColumnConfigObj.Name;

            // If the parent Scrap node is 'TABLE' type then only xpath is valid and mandatory
            if (webScrapConfigObj.Type == EWebDataConfigType.TABLE &&
                string.IsNullOrEmpty(webColumnConfigObj.XPath))
                throw new Exception("XPath must be specified for TABLE type scrapping");
            else if (webScrapConfigObj.Type == EWebDataConfigType.CSV)
            {
                if (string.IsNullOrEmpty(columnNode.Attributes["index"].Value))
                    throw new Exception("Index must be specified for CSV type scrapping");
                webColumnConfigObj.Index = Convert.ToInt32(columnNode.Attributes["index"].Value);
            }

            return webColumnConfigObj;
        }

        /// <summary>
        /// This method scraps the actual data from the webpage as per the column info
        /// </summary>
        /// <param name="count"></param>
        /// <param name="scrapConfig"></param>
        /// <param name="webnodeNavigator"></param>
        private void ColumnScrapIterator(int count, WebDataConfigScrapHtmlTable scrapConfig, XmlNode scrapNode,
            HtmlNodeNavigator webnodeNavigator)
        {
            List<DynamicTableDataInsertModel> row = new List<DynamicTableDataInsertModel>();
            bool doSkipUpdate = false;
            for (int indx = 0; indx < scrapConfig.Columns.Count; ++indx)
            {
                WebDataConfigColumn columnConfig = scrapConfig.Columns[indx];
                XPathNavigator scrappedXPathNavigator = webnodeNavigator.SelectSingleNode(columnConfig.XPath);
                ManipulateHtmlData manipulateHtml = Manipulate(columnConfig, columnConfig.Manipulations, scrappedXPathNavigator);

                DynamicTableDataInsertModel tableDataColumn = new DynamicTableDataInsertModel();

                if(columnConfig.IsUnique && string.IsNullOrEmpty(manipulateHtml.Value))
                {
                    doSkipUpdate = true;
                    break;
                }

                row.Add(tableDataColumn);

                tableDataColumn.Name = columnConfig.Name;
                tableDataColumn.IsPk = columnConfig.IsUnique;
                tableDataColumn.RowIndex = count;
                tableDataColumn.Value = manipulateHtml.Value;
                tableDataColumn.DataType =
                    WebDbContext.MetaDbConfig.TableColumnConfigs[scrapConfig.Name][columnConfig.Name].DataType;
            }

            if(!doSkipUpdate)
                WebContext.EngineContext.WebDbContext.AddOrUpdate(scrapConfig, row);
        }

        /// <summary>
        /// Manipulate the scrapped column data
        /// </summary>
        /// <param name="scrapConfig"></param>
        /// <param name="manipulateNodeList"></param>
        /// <param name="dataNode"></param>
        /// <returns></returns>
        private ManipulateHtmlData Manipulate(WebDataConfigColumn columnConfig, 
            List<WebDataConfigManipulate> manipulateNodeList, XPathNavigator dataNode)
        {
            ManipulateHtmlData result = new ManipulateHtmlData();

            string data = "";
            if (dataNode != null) data = dataNode.Value;
            if (manipulateNodeList != null && manipulateNodeList.Count > 0)
            {
                result.Value = data;
                foreach (WebDataConfigManipulate manipulate in manipulateNodeList)
                {
                    if (!string.IsNullOrEmpty(result.Value))
                    {
                        if (manipulate.Trims != null)
                        {
                            List<char> trimChars = new List<char>();
                            foreach (var trimNode in manipulate.Trims)
                            {
                                trimChars.Add(trimNode.Data[0]);
                            }

                            result.Value = result.Value.Trim(trimChars.ToArray());
                        }
                    }
                    if(!string.IsNullOrEmpty(result.Value))
                    {
                        if (manipulate.Splits != null)
                        {
                            foreach (WebDataConfigSplit splitConfig in manipulate.Splits)
                            {
                                string[] split = result.Value.Split(splitConfig.Data.ToArray());
                                result.Value = split[splitConfig.Index];
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(result.Value))
                    {
                        if (manipulate.Replaces != null)
                        {
                            foreach (WebDataConfigReplace replaceConfig in manipulate.Replaces)
                            {
                                result.Value = result.Value.Replace(replaceConfig.InString, replaceConfig.OutString);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(result.Value))
                    {
                        if (manipulate.Regexes != null)
                        {
                            foreach (WebDataConfigRegex regexConfig in manipulate.Regexes)
                            {
                                Match output = Regex.Match(result.Value,
                                    regexConfig.Pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                                result.Value = output.Groups[regexConfig.Index].Value;
                            }
                        }
                    }
                }
            }
            else
            {
                result.Value = data;
            }

            result.Url = (dataNode != null) ? dataNode.BaseURI : "";
            result.XPath = columnConfig.XPath;
            result.Value = HtmlEntity.DeEntitize(result.Value);

            return result;
        }
        
        /// <summary>
        /// Parse name attribute
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ParseName(string value)
        {
            //if(string.IsNullOrEmpty(value))
            //{
            //    value = Guid.NewGuid().ToString();
            //}

            return value;
        }

        

        /// <summary>
        /// Check the maximum level of Scrap nodes allowed is 4
        /// </summary>
        /// <param name="webScrapConfigObj">The last child Scrap node</param>
        private void CheckMaxLevelConstraint(WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            WebDataConfigScrapHtmlTable tmpObj = webScrapConfigObj;
            int level = 0;
            while(tmpObj != null)
            {
                level++;
                tmpObj = tmpObj.Parent;
            }

            if (level > 4 || level <= 0)
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_LEVEL_INVALID,
                    level.ToString());
        }

        private void CheckScrapNameAttribute(WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            bool isTableNameFound = false;
            string NameValue = null;
            WebDataConfigScrapHtmlTable tmpObj = webScrapConfigObj;

            while (tmpObj != null)
            {
                if(!string.IsNullOrEmpty(tmpObj.Name))
                {
                    if(isTableNameFound)
                        throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_MULTIPLE);
                    isTableNameFound = true;
                    NameValue = tmpObj.Name;
                }

                tmpObj = tmpObj.Parent;
            }

            if(!isTableNameFound)
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_EMPTY);
        }
    }
}
