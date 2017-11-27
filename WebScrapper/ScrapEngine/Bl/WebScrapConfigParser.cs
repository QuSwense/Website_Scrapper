using log4net;
using ScrapEngine.Bl.Parser;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System.Collections.Generic;
using System.Xml;
using WebCommon.PathHelp;
using WebReader.Xml;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The main class for parsing the scrap xml configuration file
    /// and do the actual data scrapping. 
    /// </summary>
    public class WebScrapConfigParser : IScrapParser
    {
        #region Properties

        /// <summary>
        /// The static logger per class
        /// </summary>
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
        /// Register the Scrap parsers
        /// </summary>
        private Dictionary<string, ScrapConfigParser> scrapParsers;
        
        /// <summary>
        /// Performance measurement for debug
        /// </summary>
        public PerformanceMeasure Performance { get; set; }

        /// <summary>
        /// Stores all the states for the scrap nodes
        /// </summary>
        public Dictionary<string, List<ScrapIteratorArgs>> WebScrapStates { get; set; }

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
            Performance = new PerformanceMeasure();
            WebScrapStates = new Dictionary<string, List<ScrapIteratorArgs>>();

            RegisterParsers();
        }

        /// <summary>
        /// Register all parsers
        /// </summary>
        private void RegisterParsers()
        {
            scrapParsers = new Dictionary<string, ScrapConfigParser>
            {
                { ScrapHtmlTableElement.TagName, new ScrapHtmlTableConfigParser(this) },
                { ScrapCsvElement.TagName, new ScrapCsvConfigParser(this) }
            };
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
                ParseChildScrapNodes(new ScrapIteratorArgs()
                {
                    ScrapConfigNode = rootWebdataNode
                });
            }
        }

        /// <summary>
        /// This is the parse method to parse the "Webdata" element tag
        /// </summary>
        /// <param name="rootScrapNode"></param>
        public void ParseChildScrapNodes(ScrapIteratorArgs args)
        {
            if (args.ScrapConfigNode == null || args.ScrapConfigNode.ChildNodes == null) return;

            // Read the child nodes of Scrap type nodes
            foreach (XmlNode scrapNode in args.ScrapConfigNode.ChildNodes)
            {
                CalculateRootWebDataNodePerformance(scrapNode);
                if (!ConfigScrapElementFactory(args, scrapNode)) return;
            }
        }

        /// <summary>
        /// This is the start of any Scrap type node. A scrap type node is a single entity
        /// which contains all information to scrap a website page for information, extract data,
        /// and save into table.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="parentConfig"></param>
        /// <param name="scrapNode"></param>
        /// <returns></returns>
        private bool ConfigScrapElementFactory(ScrapIteratorArgs args, XmlNode nextChildNode)
        {
            bool bProcessed = true;
            if (nextChildNode == null) bProcessed = false;

            logger.InfoFormat("Parsing Config scrap node {0}", nextChildNode.LocalName);

            if (scrapParsers.ContainsKey(nextChildNode.LocalName))
            {
                scrapParsers[nextChildNode.LocalName].Process(
                    scrapParsers[nextChildNode.LocalName].CreateArgs(args, nextChildNode));
            }
            else bProcessed = false;
            
            return bProcessed;
        }

        /// <summary>
        /// Calculate processing of Root Webdata single node
        /// </summary>
        /// <param name="xmlNode"></param>
        private void CalculateRootWebDataNodePerformance(XmlNode scrapNode)
        {
            if (scrapNode == null || scrapNode.Attributes == null) return;

            XmlAttribute idAttribute = scrapNode.Attributes["id"];
            if (idAttribute != null)
            {
                if (!string.IsNullOrEmpty(Performance.CurrentScrapNodeName) &&
                    Performance.CurrentScrapNodeName != idAttribute.Value)
                    Performance.FinalChildNode();

                if (!string.IsNullOrEmpty(Performance.CurrentScrapNodeName))
                    WebDbContext.AddMetadata(Performance.CurrentScrapNodeName, Performance);

                Performance.NewChildNode(idAttribute.Value);
            }
        }
    }
}
