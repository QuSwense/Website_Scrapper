using HtmlAgilityPack;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using SqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The business logic to parse the "Columns" Xml node and parse
    /// </summary>
    public class ScrapColumnConfigParser : IInnerBaseParser
    {
        /// <summary>
        /// Refer to the config parser
        /// </summary>
        protected WebScrapConfigParser configParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapColumnConfigParser(WebScrapConfigParser configParser)
        {
            this.configParser = configParser;
        }

        /// <summary>
        /// Process for Html nodes
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="configScrap"></param>
        /// <param name="htmlNode"></param>
        public void Process(int nodeIndex, XmlNode scrapNode, WebDataConfigScrap configScrap, HtmlNodeNavigator htmlNode)
        {
            if (configScrap.Columns.Count <= 0)
            {
                XmlNodeList columnNodeList = scrapNode.SelectNodes("Column");

                if (columnNodeList != null && columnNodeList.Count > 0)
                {
                    foreach (XmlNode columnNode in columnNodeList)
                    {
                        configScrap.Columns.Add(
                            ParseColumnElement(columnNode, configScrap));
                    }
                }

                // Load the table with partial columns in memory
                configParser.WebDbContext.AddMetadata(configScrap);
            }

            ColumnScrapIterator(scrapNode, configScrap, htmlNode);
        }

        private void Process(int nodeIndex, XmlNode scrapNode, WebDataConfigScrap configScrap, HtmlNodeNavigator htmlNode, string fileLine)
        {
            if (configScrap.Columns.Count <= 0)
            {
                XmlNodeList columnNodeList = scrapNode.SelectNodes("Column");

                if (columnNodeList != null && columnNodeList.Count > 0)
                {
                    foreach (XmlNode columnNode in columnNodeList)
                    {
                        configScrap.Columns.Add(
                            ParseColumnElement(columnNode, configScrap));
                    }
                }

                // Load the table with partial columns in memory
                configParser.WebDbContext.AddMetadata(configScrap);
            }

            ColumnScrapIterator(nodeIndex, configScrap, scrapNode, fileLine);
        }

        /// <summary>
        /// Parse the Column tag in config file
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private WebDataConfigColumn ParseColumnElement(XmlNode columnNode, WebDataConfigScrap configScrap)
        {
            var webColumnConfigObj = ParseColumnElementAttributes(columnNode, configScrap);

            // Check if manipulate tag is present
            new ScrapManipulateConfigParser(configParser).Process(columnNode, webColumnConfigObj);

            return webColumnConfigObj;
        }

        /// <summary>
        /// Parse the Config Xml Column attribute
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private WebDataConfigColumn ParseColumnElementAttributes(XmlNode columnNode, WebDataConfigScrap configScrap)
        {
            var webColumnConfigObj = 
                configParser.XmlConfigReader.ReadElement<WebDataConfigColumn>(columnNode);
            webColumnConfigObj.Parent = configScrap;
            webColumnConfigObj.Name = webColumnConfigObj.Name;

            return webColumnConfigObj;
        }

        /// <summary>
        /// This method scraps the actual data from the webpage as per the column info
        /// </summary>
        /// <param name="count"></param>
        /// <param name="scrapConfig"></param>
        /// <param name="webnodeNavigator"></param>
        private void ColumnScrapIterator(XmlNode scrapNode, WebDataConfigScrap configScrap, HtmlNodeNavigator htmlNode)
        {
            List<DynamicTableDataInsertModel> row = new List<DynamicTableDataInsertModel>();
            bool doSkipUpdate = false;
            for (int indx = 0; indx < configScrap.Columns.Count; ++indx)
            {
                WebDataConfigColumn columnConfig = configScrap.Columns[indx];
                XPathNavigator scrappedXPathNavigator = htmlNode.SelectSingleNode(columnConfig.XPath);
                ManipulateHtmlData manipulateHtml = Manipulate(columnConfig, columnConfig.Manipulations, scrappedXPathNavigator);

                DynamicTableDataInsertModel tableDataColumn = new DynamicTableDataInsertModel();

                if (columnConfig.IsUnique && string.IsNullOrEmpty(manipulateHtml.Value))
                {
                    doSkipUpdate = true;
                    break;
                }

                row.Add(tableDataColumn);

                tableDataColumn.Name = columnConfig.Name;
                tableDataColumn.IsPk = columnConfig.IsUnique;
                tableDataColumn.Value = manipulateHtml.Value;
                tableDataColumn.DataType =
                    configParser.WebDbContext.MetaDbConfig.TableColumnConfigs[configScrap.Name][columnConfig.Name].DataType;
            }

            if (!doSkipUpdate)
                configParser.WebContext.EngineContext.WebDbContext.AddOrUpdate(configScrap, row);
        }

        /// <summary>
        /// This method scraps the actual data from the webpage as per the column info
        /// </summary>
        /// <param name="count"></param>
        /// <param name="scrapConfig"></param>
        /// <param name="webnodeNavigator"></param>
        private void ColumnScrapIterator(int nodeIndex, WebDataConfigScrap scrapConfig, XmlNode scrapNode, string fileLine)
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
                    configParser.WebDbContext.MetaDbConfig.TableColumnConfigs[scrapConfig.Name][columnConfig.Name].DataType;
            }

            if (!doSkipUpdate)
                configParser.WebContext.EngineContext.WebDbContext.AddOrUpdate(scrapConfig, row);
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
                    ManipulateTrim(result, manipulate);
                    ManipulateSplit(result, manipulate);
                    ManipulateReplace(result, manipulate);
                    ManipulateRegex(result, manipulate);
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

        private void ManipulateRegex(ManipulateHtmlData result, WebDataConfigManipulate manipulate)
        {

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

        private void ManipulateReplace(ManipulateHtmlData result, WebDataConfigManipulate manipulate)
        {
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
        }

        private void ManipulateSplit(ManipulateHtmlData result, WebDataConfigManipulate manipulate)
        {
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
        }

        private void ManipulateTrim(ManipulateHtmlData result, WebDataConfigManipulate manipulate)
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
        }
    }
}
