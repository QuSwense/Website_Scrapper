using DynamicDatabase.Model;
using HtmlAgilityPack;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using WebReader.Xml;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The main class for parsing the scrap xml configuration file
    /// and do the scrapping
    /// </summary>
    public class WebScrapConfigParser
    {
        /// <summary>
        /// Reference to the parent Web context object
        /// </summary>
        public WebScrapHtmlContext WebContext { get; protected set; }
        
        /// <summary>
        /// A xml configuration reader
        /// </summary>
        private DXmlDocReader xmlConfigReader;

        /// <summary>
        /// The Html helper command class
        /// </summary>
        private HtmlScrapperCommand ScrapperCommand;

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
            WebContext.EngineContext.AppTopicPath.AppTopicScrap.AssertExists();
            xmlConfigReader = DXmlDocReader.Create(WebContext.EngineContext.AppTopicPath.AppTopicScrap.FullPath);

            // Read the Scraps nodes which are the individual reader config nodes
            foreach (XmlNode rootScrapNode in xmlConfigReader.ReadNodes("//WebData/Scrap"))
                ParseScrapElement(rootScrapNode);
        }

        /// <summary>
        /// Parse Root Scrap Element
        /// </summary>
        /// <param name="reader"></param>
        private void ParseScrapElement(XmlNode scrapNode, WebDataConfigScrap parentScrapConfigObj = null,
            HtmlNodeNavigator webNodeNavigator = null)
        {
            // Construct the scrap element state
            WebDataConfigScrap webScrapConfigObj = ParseScrapElementAttributes(scrapNode, parentScrapConfigObj, webNodeNavigator);

            // If the scrap element type is TABLE type that means
            // The child data needs to be processed in a loop
            if (webScrapConfigObj.Type == EWebDataConfigType.TABLE)
            {
                // This finally scraps the html webpage data
                List<HtmlNodeNavigator> webChildNodeNavigators = ProcessHtmlTableScrapElement(webScrapConfigObj);
                ProcessAsHtmlTableType(webScrapConfigObj, scrapNode, webChildNodeNavigators);
            }
        }

        /// <summary>
        /// Parse scrap element attributes
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="parentScrapConfigObj"></param>
        /// <returns></returns>
        private WebDataConfigScrap ParseScrapElementAttributes(XmlNode scrapNode,
            WebDataConfigScrap parentScrapConfigObj, HtmlNodeNavigator webNodeNavigator = null)
        {
            WebDataConfigScrap webScrapConfigObj = xmlConfigReader.ReadElement<WebDataConfigScrap>(scrapNode);
            webScrapConfigObj.Parent = parentScrapConfigObj;
            webScrapConfigObj.Name = ParseName(webScrapConfigObj.Name);
            webScrapConfigObj.Url = ParseUrlValue(webScrapConfigObj.Url, webNodeNavigator, parentScrapConfigObj);
            return webScrapConfigObj;
        }

        /// <summary>
        /// Process the parsed Scrap element tag
        /// This method uses the xml config Scrap information to get to the Html webpage path
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        private List<HtmlNodeNavigator> ProcessHtmlTableScrapElement(WebDataConfigScrap webScrapConfigObj)
        {
            HtmlNode htmlDoc = ScrapperCommand.Load(webScrapConfigObj.Url);
            return ScrapperCommand.ReadNodes(htmlDoc, webScrapConfigObj.XPath);
        }

        /// <summary>
        /// Process the Scrap element data as HTML Table type
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="scrapNode"></param>
        private void ProcessAsHtmlTableType(WebDataConfigScrap webScrapConfigObj, XmlNode scrapNode,
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

            WebContext.EngineContext.WebDbContext.WebScrapDb.Commit();
        }

        /// <summary>
        /// Parse and Process any Child Scrap Nodes (recursively)
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="scrapNode"></param>
        /// <param name="webNodeNavigator"></param>
        private void ProcessChildScrapNodes(WebDataConfigScrap webScrapConfigObj, XmlNode scrapNode,
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
        private void ProcessColumnNodes(int nodeIndex, WebDataConfigScrap webScrapConfigObj, XmlNode scrapNode,
            HtmlNodeNavigator webNodeNavigator)
        {
            if (webScrapConfigObj.Columns.Count <= 0)
            {
                XmlNodeList columnNodeList = scrapNode.SelectNodes("Column");

                if (columnNodeList != null && columnNodeList.Count > 0)
                {
                    foreach (XmlNode columnNode in columnNodeList)
                    {
                        webScrapConfigObj.Columns.Add(
                            ParseColumnElement(nodeIndex, columnNode, webScrapConfigObj, webNodeNavigator));
                    }
                }
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
        private WebDataConfigColumn ParseColumnElement(int nodeIndex, XmlNode columnNode, WebDataConfigScrap webScrapConfigObj, HtmlNodeNavigator item)
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
                    webManipulateConfigObj.Splits.Add(xmlConfigReader.ReadElement<WebDataConfigSplit>(splitNode));
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
                    webManipulateConfigObj.Trims.Add(xmlConfigReader.ReadElement<WebDataConfigTrim>(trimNode));
                }
            }
        }

        /// <summary>
        /// Parse the Config Xml Column attribute
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private WebDataConfigColumn ParseColumnElementAttributes(XmlNode columnNode, WebDataConfigScrap webScrapConfigObj)
        {
            WebDataConfigColumn webColumnConfigObj = xmlConfigReader.ReadElement<WebDataConfigColumn>(columnNode);
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
        private void ColumnScrapIterator(int count, WebDataConfigScrap scrapConfig, XmlNode scrapNode,
            HtmlNodeNavigator webnodeNavigator)
        {
            List<TableDataColumnModel> row = new List<TableDataColumnModel>();
            for (int indx = 0; indx < scrapConfig.Columns.Count; ++indx)
            {
                WebDataConfigColumn columnConfig = scrapConfig.Columns[indx];
                XPathNavigator scrappedXPathNavigator = webnodeNavigator.SelectSingleNode(columnConfig.XPath);
                ManipulateHtmlData manipulateHtml = Manipulate(columnConfig, columnConfig.Manipulations, scrappedXPathNavigator);
                
                TableDataColumnModel tableDataColumn = new TableDataColumnModel();
                row.Add(tableDataColumn);

                tableDataColumn.Name = columnConfig.Name;
                tableDataColumn.IsPk = columnConfig.IsPk;
                tableDataColumn.RowIndex = count;
                tableDataColumn.Value = manipulateHtml.Value;
                tableDataColumn.Url = manipulateHtml.Url;
                tableDataColumn.XPath = manipulateHtml.XPath;
            }

            WebContext.EngineContext.WebDbContext.AddOrUpdate(scrapConfig.Name, row);
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
                foreach (WebDataConfigManipulate manipulate in manipulateNodeList)
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (manipulate.Trims != null)
                        {
                            foreach (var trimNode in manipulate.Trims)
                            {
                                data = data.Trim(trimNode.Data.ToCharArray());
                            }
                        }
                        if (manipulate.Splits != null)
                        {
                            foreach (WebDataConfigSplit splitConfig in manipulate.Splits)
                            {
                                string[] split = data.Split(splitConfig.Data.ToArray());
                                result.Value += split[splitConfig.Index];
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
            if(string.IsNullOrEmpty(value))
            {
                value = Guid.NewGuid().ToString();
            }

            return value;
        }

        /// <summary>
        /// Parse url value
        /// </summary>
        /// <param name="urlValue"></param>
        /// <param name="scrapNode"></param>
        /// <returns></returns>
        private string ParseUrlValue(string urlValue, HtmlNodeNavigator scrapNode = null,
            WebDataConfigScrap parentScrapNode = null)
        {
            if (string.IsNullOrEmpty(urlValue)) return urlValue;
            if (!urlValue.StartsWith("@")) return urlValue;
            if (scrapNode == null) return urlValue;

            if(urlValue.Contains("{parentValue}"))
            {
                if (scrapNode == null) return urlValue;
                XPathNavigator htmlNode = scrapNode.SelectSingleNode(parentScrapNode.XPath);

                if(htmlNode != null)
                {
                    urlValue = htmlNode.Value;
                }
            }

            return urlValue;
        }
    }
}
