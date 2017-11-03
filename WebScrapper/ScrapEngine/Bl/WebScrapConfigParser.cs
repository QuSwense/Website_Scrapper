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
        /// The xml document is loaded and stored in memory
        /// </summary>
        private XmlDocument xmlDocument;

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
        public WebScrapConfigParser(WebScrapHtmlContext context)
        {
            WebContext = context;
        }

        /// <summary>
        /// The method to call by the implementor.
        /// This method starts the scrapping of data
        /// </summary>
        public void Run()
        {
            // Initialize the data store
            WebScrapsConfig = new Dictionary<string, WebDataConfigScrap>();

            // Load the scrap xml config
            string scrapXmlConfigFile = ConfigPathHelper.GetScrapConfigPath(WebContext.EngineContext.AppTopic);
            if (string.IsNullOrEmpty(scrapXmlConfigFile)) throw new Exception("The scrap xml config file is not found");
            xmlDocument = new XmlDocument();
            xmlDocument.Load(scrapXmlConfigFile);

            // Read the Scraps nodes which are the individual reader config nodes
            XmlNodeList rootScrapNodeList = xmlDocument.DocumentElement.SelectNodes("/WebData/Scrap");

            foreach (XmlNode rootScrapNode in rootScrapNodeList)
            {
                ParseScrapElement(rootScrapNode);
            }
        }

        /// <summary>
        /// Parse Root Scrap Element
        /// </summary>
        /// <param name="reader"></param>
        private void ParseScrapElement(XmlNode scrapNode, WebDataConfigScrap parentScrapNode = null, HtmlNodeNavigator webnode = null)
        {
            // Construct the scrap element state
            WebDataConfigScrap webScrapConfigObj = ParseScrapElementAttributes(scrapNode, parentScrapNode, webnode);
            WebScrapsConfig.Add(webScrapConfigObj.Name, webScrapConfigObj);

            ProcessScrapElement(webScrapConfigObj);

            // if the scrap element type is TABLE type that means
            // The child data needs to be processed in a loop
            if (webScrapConfigObj.Type == EWebDataConfigType.TABLE)
            {
                int nodeIndex = 0;
                foreach (var item in webScrapConfigObj.State.Nodes)
                {
                    // Read the Scraps nodes which are the individual reader config nodes
                    XmlNodeList childScrapNodeList = scrapNode.SelectNodes("Scrap");

                    if (childScrapNodeList != null && childScrapNodeList.Count > 0)
                    {
                        foreach (XmlNode childScrapNode in childScrapNodeList)
                        {
                            ParseScrapElement(childScrapNode, webScrapConfigObj, item);
                        }
                    }

                    // Read the Scraps nodes which are the individual reader config nodes
                    XmlNodeList columnNodeList = scrapNode.SelectNodes("Column");

                    if(columnNodeList != null && columnNodeList.Count > 0)
                    {
                        webScrapConfigObj.Columns = new List<WebDataConfigColumn>();
                        foreach (XmlNode columnNode in columnNodeList)
                        {
                            webScrapConfigObj.Columns.Add(
                                ParseColumnElement(nodeIndex, columnNode, webScrapConfigObj, item));
                        }

                        ColumnScrapIterator(nodeIndex, webScrapConfigObj, item);
                    }

                    nodeIndex++;
                }
            }

            ParseHtmlTableFromWeb(webScrapConfigObj);
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
        /// Parse scrap element attributes
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="parentScrapNode"></param>
        /// <returns></returns>
        private WebDataConfigScrap ParseScrapElementAttributes(XmlNode scrapNode, WebDataConfigScrap parentScrapNode, HtmlNodeNavigator item = null)
        {
            WebDataConfigScrap webScrapConfigObj = new WebDataConfigScrap();
            webScrapConfigObj.Parent = parentScrapNode;

            webScrapConfigObj.Name = ParseName(scrapNode.Attributes["name"].Value);
            webScrapConfigObj.DbTable = scrapNode.Attributes["dbtbl"].Value;
            webScrapConfigObj.Url = ParseUrlValue(scrapNode.Attributes["url"].Value, item);
            webScrapConfigObj.XPath = scrapNode.Attributes["xpath"].Value;
            webScrapConfigObj.Type =
                (EWebDataConfigType)Enum.Parse(typeof(EWebDataConfigType), scrapNode.Attributes["type"].Value);
            return webScrapConfigObj;
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

        /// <summary>
        /// Process the parsed Scrap element tag
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        private void ProcessScrapElement(WebDataConfigScrap webScrapConfigObj)
        {
            webScrapConfigObj.State = new WebDataConfigScrapState();

            HtmlNode htmlDoc = ScrapperCommand.LoadOnline(webScrapConfigObj.Url);
            var navigator = (HtmlNodeNavigator)htmlDoc.CreateNavigator();
            webScrapConfigObj.State.Nodes = navigator.Select(webScrapConfigObj.XPath).Cast<HtmlNodeNavigator>().ToList();
        }
    }
}
