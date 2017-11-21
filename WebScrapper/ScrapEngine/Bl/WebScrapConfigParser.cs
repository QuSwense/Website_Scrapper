using HtmlAgilityPack;
using log4net;
using ScrapEngine.Bl.Parser;
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
        #region Properties

        public static ILog logger = LogManager.GetLogger(typeof(WebScrapConfigParser));

        /// <summary>
        /// Reference to the parent Web context object
        /// </summary>
        public IScrapHtmlContext WebContext { get; protected set; }

        /// <summary>
        /// A xml configuration reader
        /// </summary>
        public DXmlDocReader XmlConfigReader { get; protected set; }

        /// <summary>
        /// The Html helper command class
        /// </summary>
        public HtmlScrapperCommand ScrapperCommand { get; protected set; }

        /// <summary>
        /// The scrap html table config parser
        /// </summary>
        private ScrapHtmlTableConfigParser scrapHtmlTableConfigParser;

        /// <summary>
        /// Scrap csv config parser
        /// </summary>
        private ScrapCsvConfigParser scrapCsvConfigParser;

        #endregion Properties

        #region Helper Properties

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
        /// Get application config
        /// </summary>
        public ApplicationConfig AppConfig
        {
            get { return WebContext.EngineContext.AppConfig; }
        }

        #endregion Helper Properties

        /// <summary>
        /// Constructor
        /// </summary>
        public WebScrapConfigParser()
        {
            scrapHtmlTableConfigParser = new ScrapHtmlTableConfigParser(this);
            scrapCsvConfigParser = new ScrapCsvConfigParser(this);
        }

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
                logger.InfoFormat("Parsing WebData xml node from {0} Xml config {1}", 
                    AppTopicPath.AppTopic, AppTopicPath.AppTopicScrap.FullPath);
                ParseChildScrapNodes(rootWebdataNode, null, null);
            }
        }

        /// <summary>
        /// This is the parse method to parse the "Webdata" element tag
        /// </summary>
        /// <param name="rootScrapNode"></param>
        public void ParseChildScrapNodes(XmlNode xmlNode, ScrapElement parentConfig,
            HtmlNodeNavigator webNodeNavigator)
        {
            if (xmlNode == null || xmlNode.ChildNodes == null) return;

            // Read the child nodes of Scrap type nodes
            foreach (XmlNode scrapNode in xmlNode.ChildNodes)
                if (!ConfigScrapElementFactory(scrapNode, parentConfig, webNodeNavigator)) return;
        }

        /// <summary>
        /// This is the start of any Scrap type node. A scrap type node is a single entity
        /// which contains all information to scrap a website page for information, extract data,
        /// and save into table
        /// </summary>
        /// <param name="scrapTypeNode"></param>
        private bool ConfigScrapElementFactory(XmlNode xmlNode, ScrapElement parentConfig,
            HtmlNodeNavigator scrapNode)
        {
            bool bProcessed = true;
            if (xmlNode == null) bProcessed = false;

            logger.InfoFormat("Parsing Config scrap node {0}", xmlNode.LocalName);

            if (xmlNode.LocalName == ScrapHtmlTableElement.TagName)
                scrapHtmlTableConfigParser.Process(xmlNode, parentConfig, scrapNode);
            else if (xmlNode.LocalName == WebDataConfigScrapCsv.TagName)
                scrapCsvConfigParser.Process(xmlNode, parentConfig, scrapNode);
            else bProcessed = false;

            return bProcessed;
        }
    }
}
