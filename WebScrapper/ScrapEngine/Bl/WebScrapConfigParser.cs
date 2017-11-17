using HtmlAgilityPack;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapEngine.Model.State;
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
                ParseChildScrapNodes(rootWebdataNode, null, null);
            }
        }

        /// <summary>
        /// This is the parse method to parse the "Webdata" element tag
        /// </summary>
        /// <param name="rootScrapNode"></param>
        public void ParseChildScrapNodes(XmlNode xmlNode, WebDataConfigScrap parentConfig,
            HtmlNodeNavigator webNodeNavigator)
        {
            if (xmlNode == null || xmlNode.ChildNodes == null) return;

            // Read the child nodes of Scrap type nodes
            foreach (XmlNode scrapNode in xmlNode.ChildNodes)
            {
                ConfigScrapElementFactory(scrapNode, parentConfig, webNodeNavigator);
            }
        }

        /// <summary>
        /// This is the start of any Scrap type node. A scrap type node is a single entity
        /// which contains all information to scrap a website page for information, extract data,
        /// and save into table
        /// </summary>
        /// <param name="scrapTypeNode"></param>
        private void ConfigScrapElementFactory(XmlNode xmlNode, WebDataConfigScrap parentConfig,
            HtmlNodeNavigator scrapNode)
        {
            if (scrapNode == null) return;
            if (scrapNode.LocalName == WebDataConfigScrapHtmlTable.TagName)
                new ScrapHtmlTableConfigParser(this).Process(xmlNode, parentConfig, scrapNode);
            else if (scrapNode.LocalName == WebDataConfigScrapCsv.TagName)
                new ScrapCsvConfigParser(this).Process(xmlNode, parentConfig, scrapNode);
        }
    }
}
