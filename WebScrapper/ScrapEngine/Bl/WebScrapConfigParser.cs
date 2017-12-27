using log4net;
using ScrapEngine.Bl.Parser;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System.Collections.Generic;
using ScrapEngine.Common;
using ScrapEngine.Model.Scrap;
using ConfigPathHelper;

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
        /// The Html helper command class
        /// </summary>
        public HtmlScrapperCommand ScrapperCommand { get; protected set; }

        /// <summary>
        /// Register the Scrap parsers
        /// </summary>
        private Dictionary<string, ScrapConfigParser> scrapParsers;

        /// <summary>
        /// Stores the state of the parsers
        /// </summary>
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
                logger.InfoFormat("Parsing Scrap xml node from {0} Xml config {1}",
                    AppTopicPath.AppTopicName, AppTopicPath.AppTopicScrap.FullPath);

                if (string.IsNullOrEmpty(rootScrapObj.IdString))
                    throw new System.Exception();

                Performance.NewChildNode(rootScrapObj.IdString);
                
                StateModel.AddRootScrapArgsState(GetScrapIteratorArgs(rootScrapObj));

                if (!ConfigScrapElementFactory()) return;

                Performance.FinalChildNode();

                if (!string.IsNullOrEmpty(Performance.CurrentScrapNodeName))
                    WebDbContext.AddMetadata(Performance.CurrentScrapNodeName, Performance);
            }
        }

        /// <summary>
        /// Process scrap node
        /// </summary>
        /// <param name="configElement"></param>
        private void ProcessScrapNode(IConfigElement configElement)
        {
            Performance.NewChildNode(configElement.IdScrapUnit);

            StateModel.AddRootScrapArgsState(GetScrapIteratorArgs(configElement));

            if (!ConfigScrapElementFactory()) return;

            Performance.FinalChildNode();
        }

        /// <summary>
        /// This is the parse method to parse the "Webdata" element tag
        /// </summary>
        /// <param name="rootScrapNode"></param>
        public void ParseChildScrapNodes()
        {
            if (StateModel.CurrentScrapIteratorArgs.ScrapConfigObj.Scraps != null)
            {
                // Check if there are any more Scrap Child nodes
                foreach (ScrapElement scrapObj in StateModel.CurrentScrapIteratorArgs.ScrapConfigObj.Scraps)
                {
                    StateModel.AddNewScrap(GetScrapIteratorArgs(scrapObj));
                    if (!ConfigScrapElementFactory()) return;
                    StateModel.RestoreScrap();
                }
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
        private bool ConfigScrapElementFactory()
        {
            bool bProcessed = true;

            ScrapConfigParser configParser = GetCurrentParser();

            if (configParser != null) configParser.Process();
            else bProcessed = false;

            return bProcessed;
        }
        
        /// <summary>
        /// Retrieve the current parser
        /// </summary>
        /// <returns></returns>
        private ScrapConfigParser GetCurrentParser()
        {
            if (StateModel.CurrentScrapIteratorArgs.ScrapConfigObj is ScrapHtmlTableElement)
                return scrapParsers[ScrapXmlConsts.ScrapHtmlTableNodeName];
            else if (StateModel.CurrentScrapIteratorArgs.ScrapConfigObj is ScrapCsvElement)
                return scrapParsers[ScrapXmlConsts.ScrapCsvNodeName];
            else if (StateModel.CurrentScrapIteratorArgs.ScrapConfigObj is ScrapXmlElement)
                return scrapParsers[ScrapXmlConsts.ScrapXmlNodeName];
            else
                return null;
        }

        /// <summary>
        /// Get the Scrap interator for the Scrap element type
        /// </summary>
        /// <param name="scrapObj"></param>
        /// <returns></returns>
        private ScrapIteratorArgs GetScrapIteratorArgs(ScrapElement scrapObj)
        {
            return new ScrapIteratorArgs()
            {
                ScrapConfigObj = scrapObj
            };
        }
    }
}
