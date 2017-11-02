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
        private void ParseScrapElement(XmlNode scrapNode, WebDataConfigScrap parentScrapNode = null)
        {
            WebDataConfigScrap webScrapConfigObj = ParseScrapElementAttributes(scrapNode, parentScrapNode);
            WebScrapsConfig.Add(webScrapConfigObj.Name, webScrapConfigObj);

            ProcessScrapElement(webScrapConfigObj);

            // Read the Scraps nodes which are the individual reader config nodes
            XmlNodeList childScrapNodeList = xmlDocument.DocumentElement.SelectNodes("/WebData/Scrap");

            foreach (XmlNode childScrapNode in childScrapNodeList)
            {
                ParseScrapElement(childScrapNode, webScrapConfigObj);
            }

            if (webScrapConfigObj.Type == EWebDataConfigType.TABLE)
                ParseHtmlTableFromWeb(webScrapConfigObj);
        }

        /// <summary>
        /// Parse scrap element attributes
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="parentScrapNode"></param>
        /// <returns></returns>
        private WebDataConfigScrap ParseScrapElementAttributes(XmlNode scrapNode, WebDataConfigScrap parentScrapNode)
        {
            WebDataConfigScrap webScrapConfigObj = new WebDataConfigScrap();
            webScrapConfigObj.Parent = parentScrapNode;

            webScrapConfigObj.Name = ParseName(scrapNode.Attributes["name"].Value);
            webScrapConfigObj.DbTable = scrapNode.Attributes["dbtbl"].Value;
            webScrapConfigObj.Url = ParseUrlValue(scrapNode.Attributes["url"].Value, webScrapConfigObj);
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
            XmlNodeList childColumnNodeList = webScrapConfigObj.State.SelectNodes("/WebData/Scrap");

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
        private string ParseUrlValue(string urlValue, WebDataConfigScrap scrapNode = null)
        {
            if (string.IsNullOrEmpty(urlValue)) return urlValue;
            if (!urlValue.StartsWith("@")) return urlValue;
            if (scrapNode == null) return urlValue;

            if(urlValue.Contains("{parentValue}"))
            {
                if (scrapNode.Parent == null) return urlValue;
                HtmlNodeNavigator htmlNodeNavigator = scrapNode.Parent.State.Nodes[0];
                XPathNavigator htmlNode = htmlNodeNavigator.SelectSingleNode(scrapNode.XPath);

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
