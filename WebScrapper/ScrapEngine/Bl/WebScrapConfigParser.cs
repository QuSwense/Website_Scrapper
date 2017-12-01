using log4net;
using ScrapEngine.Bl.Parser;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System.Collections.Generic;
using System.Xml;
using WebCommon.PathHelp;
using WebReader.Xml;
using System;
using ScrapEngine.Common;
using WebCommon.Error;

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

        public ScrapIteratorStateModel StateModel { get; set; }

        /// <summary>
        /// Performance measurement for debug
        /// </summary>
        public PerformanceMeasure Performance { get; set; }

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

        public List<ScrapElement> RootScrapNodes
        {
            get { return WebContext.RootScrapNodes; }
        }

        #endregion Helper Properties

        /// <summary>
        /// Constructor
        /// </summary>
        public WebScrapConfigParser()
        {
            Performance = new PerformanceMeasure();
            StateModel = new ScrapIteratorStateModel();

            RegisterParsers();
        }

        /// <summary>
        /// Register all parsers
        /// </summary>
        private void RegisterParsers()
        {
            scrapParsers = new Dictionary<string, ScrapConfigParser>
            {
                { ScrapXmlConsts.ScrapHtmlTableNodeName, new ScrapHtmlTableConfigParser(this) },
                { ScrapXmlConsts.ScrapCsvNodeName, new ScrapCsvConfigParser(this) }
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
            // Read the Webdata node which is the start node of any app topic
            foreach (ScrapElement rootScrapObj in RootScrapNodes)
            {
                logger.InfoFormat("Parsing WebData xml node from {0} Xml config {1}",
                    AppTopicPath.AppTopic, AppTopicPath.AppTopicScrap.FullPath);
                CalculateRootWebDataNodePerformance(rootScrapObj);
                ResetIfNew(rootScrapObj);

                StateModel.AddRootScrapArgsState(new ScrapIteratorArgs()
                {
                    ScrapConfigObj = rootScrapObj
                });

                if (!ConfigScrapElementFactory(StateModel.WebScrapArgs[rootScrapObj.IdString])) return;
            }
        }

        /// <summary>
        /// This is the parse method to parse the "Webdata" element tag
        /// </summary>
        /// <param name="rootScrapNode"></param>
        public void ParseChildScrapNodes(ScrapIteratorArgs scrapIteratorArgs)
        {
            // Check if there are any more Scrap Child nodes
            foreach (ScrapElement scrapObj in scrapIteratorArgs.ScrapConfigObj.Scraps)
            {
                scrapIteratorArgs.ScrapConfigObj = scrapObj;
                if (!ConfigScrapElementFactory(scrapIteratorArgs)) return;
            }
        }

        private void ResetIfNew(ScrapElement rootScrapObj)
        {
            if (!string.IsNullOrEmpty(Performance.CurrentScrapNodeName) &&
                    Performance.CurrentScrapNodeName != rootScrapObj.IdString)
            {
                foreach (var parserkv in scrapParsers)
                    parserkv.Value.Reset();
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
        private bool ConfigScrapElementFactory(ScrapIteratorArgs scrapIteratorArgs)
        {
            bool bProcessed = true;

            ScrapConfigParser configParser = GetParser(scrapIteratorArgs.ScrapConfigObj);

            if (configParser != null) configParser.Process(scrapIteratorArgs);
            else bProcessed = false;

            return bProcessed;
        }

        /// <summary>
        /// This method calculates the total time taken for processing a single root level Scrap node
        /// This method identifies a new root Scrap node by the 'id' attribute value
        /// </summary>
        /// <param name="xmlNode"></param>
        private void CalculateRootWebDataNodePerformance(ScrapElement rootScrapObj)
        {
            if (!string.IsNullOrEmpty(Performance.CurrentScrapNodeName) &&
                    Performance.CurrentScrapNodeName != rootScrapObj.IdString)
                Performance.FinalChildNode();

            if (!string.IsNullOrEmpty(Performance.CurrentScrapNodeName))
                WebDbContext.AddMetadata(Performance.CurrentScrapNodeName, Performance);

            Performance.NewChildNode(rootScrapObj.IdString);
        }

        private ScrapConfigParser GetParser(ScrapElement scrapObj)
        {
            if (scrapObj is ScrapHtmlTableElement)
                return scrapParsers[ScrapXmlConsts.ScrapHtmlTableNodeName];
            else if (scrapObj is ScrapCsvElement)
                return scrapParsers[ScrapXmlConsts.ScrapCsvNodeName];
            else
                return null;
        }
    }
}
