using DynamicDatabase.Model;
using HtmlAgilityPack;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using WebCommon.Config;
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
        /// A store to save named Scrap element nodes
        /// </summary>
        public Dictionary<string, WebDataConfigScrap> WebScrapsConfig { get; set; }

        /// <summary>
        /// The html page is loaded as xml document and stored in memory
        /// </summary>
        private XmlDocument xmlDocument;

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
            WebScrapsConfig = new Dictionary<string, WebDataConfigScrap>();
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
            foreach (XmlNode rootScrapNode in xmlConfigReader.ReadNodes("/WebData/Scrap"))
                ParseScrapElement(rootScrapNode);
        }

        /// <summary>
        /// Parse Root Scrap Element
        /// </summary>
        /// <param name="reader"></param>
        private void ParseScrapElement(XmlNode scrapNode, WebDataConfigScrap parentScrapNode = null,
            HtmlNodeNavigator webNodeNavigator = null)
        {
            // Construct the scrap element state
            WebDataConfigScrap webScrapConfigObj = ParseScrapElementAttributes(scrapNode, parentScrapNode, webNodeNavigator);
            WebScrapsConfig.Add(webScrapConfigObj.Name, webScrapConfigObj);

            // This finally scraps the html webpage data
            List<HtmlNodeNavigator> webChildNodeNavigator = ProcessScrapElement(webScrapConfigObj);

            // If the scrap element type is TABLE type that means
            // The child data needs to be processed in a loop
            if (webScrapConfigObj.Type == EWebDataConfigType.TABLE)
            {
                ProcessAsHtmlTableType(webScrapConfigObj, scrapNode, webChildNodeNavigator);
            }

            ParseHtmlTableFromWeb(webScrapConfigObj);
        }

        /// <summary>
        /// Parse scrap element attributes
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="parentScrapNode"></param>
        /// <returns></returns>
        private WebDataConfigScrap ParseScrapElementAttributes(XmlNode scrapNode,
            WebDataConfigScrap parentScrapNode, HtmlNodeNavigator webNodeNavigator = null)
        {
            WebDataConfigScrap webScrapConfigObj = xmlConfigReader.ReadElement<WebDataConfigScrap>(scrapNode);
            webScrapConfigObj.Parent = parentScrapNode;
            webScrapConfigObj.Name = ParseName(webScrapConfigObj.Name);
            webScrapConfigObj.Url = ParseUrlValue(webScrapConfigObj.Url, webNodeNavigator);
            return webScrapConfigObj;
        }

        /// <summary>
        /// Process the parsed Scrap element tag
        /// This method uses the xml config Scrap information to get to the Html webpage path
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        private List<HtmlNodeNavigator> ProcessScrapElement(WebDataConfigScrap webScrapConfigObj)
        {
            HtmlNode htmlDoc = ScrapperCommand.LoadOnline(webScrapConfigObj.Url);
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
            XmlNodeList columnNodeList = scrapNode.SelectNodes("Column");

            if (columnNodeList != null && columnNodeList.Count > 0)
            {
                foreach (XmlNode columnNode in columnNodeList)
                {
                    webScrapConfigObj.Columns.Add(
                        ParseColumnElement(nodeIndex, columnNode, webScrapConfigObj, webNodeNavigator));
                }

                ColumnScrapIterator(nodeIndex, webScrapConfigObj, webNodeNavigator);
            }
        }

        private void ColumnScrapIterator(int count, WebDataConfigScrap scrapConfig, 
            HtmlNodeNavigator webnodeNavigator)
        {
            List<TableDataColumnModel> row = new List<TableDataColumnModel>();
            for (int indx = 0; indx < scrapConfig.Columns.Count; ++indx)
            {
                WebDataConfigColumn columnConfig = scrapConfig.Columns[indx];
                ManipulateHtmlData manipulateHtml = null;

                XPathNavigator scrappedXPathNavigator = webnodeNavigator.SelectSingleNode(columnConfig.XPath);

                // Read the Manipulate elements if any
                XmlNodeList manipulateNodeList = columnConfig.State.SelectNodes("Manipulate");

                if (manipulateNodeList != null && manipulateNodeList.Count > 0)
                {
                    ManipulateHtmlData manipulatedData = Manipulate(scrapConfig, manipulateNodeList, scrappedXPathNavigator);
                }

                TableDataColumnModel tableDataColumn = new TableDataColumnModel();
                row.Add(tableDataColumn);

                tableDataColumn.Name = columnConfig.Name;
                tableDataColumn.IsPk = columnConfig.IsPk;
                tableDataColumn.Value = manipulateHtml.Value;
                tableDataColumn.RowIndex = count;
                tableDataColumn.Url = manipulateHtml.Url;
                tableDataColumn.XPath = manipulateHtml.XPath;
            }

            WebContext.EngineContext.WebDbContext.WebScrapDb.AddOrUpdate(scrapConfig.Name, row);
        }

        private WebDataConfigColumn ParseColumnElement(int nodeIndex, XmlNode columnNode, WebDataConfigScrap webScrapConfigObj, HtmlNodeNavigator item)
        {
            WebDataConfigColumn webColumnConfigObj = ParseColumnElementAttributes(columnNode, webScrapConfigObj);

            

            return webColumnConfigObj;
        }

        private ManipulateHtmlData Manipulate(WebDataConfigScrap scrapConfig, XmlNodeList manipulateNodeList, XPathNavigator dataNode)
        {
            ManipulateHtmlData result = new ManipulateHtmlData();

            string data = "";
            if (dataNode != null) data = dataNode.Value;
            if (manipulateNodeList != null)
            {
                foreach (WebDataConfigManipulate manipulate in manipulateNodeList)
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (manipulate.Trim != null)
                        {
                            data = data.Trim(manipulate.Trim.Data.ToCharArray());
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

            result.Url = dataNode.BaseURI;
            result.XPath = scrapConfig.XPath;
            result.Value = HtmlEntity.DeEntitize(result.Value);

            return result;
        }

        private WebDataConfigColumn ParseColumnElementAttributes(XmlNode columnNode, WebDataConfigScrap webScrapConfigObj)
        {
            WebDataConfigColumn webColumnConfigObj = new WebDataConfigColumn();
            webColumnConfigObj.Parent = webScrapConfigObj;

            if (string.IsNullOrEmpty(columnNode.Attributes["name"].Value))
                throw new Exception("Column Name must be specified");
            webColumnConfigObj.Name = ParseColumnName(columnNode.Attributes["name"].Value);
            webColumnConfigObj.IsPk = Convert.ToBoolean(columnNode.Attributes["ispk"].Value);

            // If the parent Scrap node is 'TABLE' type then only xpath is valid and mandatory
            if(webScrapConfigObj.Type == EWebDataConfigType.TABLE)
            {
                if (string.IsNullOrEmpty(columnNode.Attributes["xpath"].Value))
                    throw new Exception("XPath must be specified for TABLE type scrapping");
                webColumnConfigObj.XPath = columnNode.Attributes["xpath"].Value;
            }
            else if(webScrapConfigObj.Type == EWebDataConfigType.CSV)
            {
                if (string.IsNullOrEmpty(columnNode.Attributes["index"].Value))
                    throw new Exception("Index must be specified for CSV type scrapping");
                webColumnConfigObj.Index = Convert.ToInt32(columnNode.Attributes["index"].Value);
            }

            webColumnConfigObj.State = columnNode;

            return webColumnConfigObj;
        }

        private string ParseColumnName(string value)
        {
            
            return value;
        }

        /// <summary>
        /// Parse html table from web
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        private void ParseHtmlTableFromWeb(WebDataConfigScrap webScrapConfigObj)
        {
            // Read the Scraps nodes which are the individual reader config nodes
            XmlNodeList childColumnNodeList = webScrapConfigObj.State.Nodes.SelectNodes("/WebData/Scrap");

            foreach (XmlNode childScrapNode in childScrapNodeList)
            {
                ParseScrapElement(childScrapNode, webScrapConfigObj);
            }
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
        private string ParseUrlValue(string urlValue, HtmlNodeNavigator scrapNode = null)
        {
            if (string.IsNullOrEmpty(urlValue)) return urlValue;
            if (!urlValue.StartsWith("@")) return urlValue;
            if (scrapNode == null) return urlValue;

            if(urlValue.Contains("{parentValue}"))
            {
                if (scrapNode == null) return urlValue;
                XPathNavigator htmlNode = scrapNode.SelectSingleNode(scrapNode.XPath);

                if(htmlNode != null)
                {
                    urlValue = htmlNode.Value;
                }
            }

            return urlValue;
        }
    }
}
