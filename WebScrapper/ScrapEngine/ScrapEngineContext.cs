using log4net;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using WebCommon.PathHelp;
using WebReader.Xml;

namespace ScrapEngine
{
    /// <summary>
    /// The main controller class which controls the web scrapping of web page data from
    /// the websites and stores in the database. The Scrapping rule configuration is in
    /// the config xml file.
    /// This context class is to be used per Application Topic
    /// </summary>
    public class ScrapEngineContext : IScrapEngineContext
    {
        #region Properties

        /// <summary>
        /// The logger
        /// </summary>
        public static ILog logger = LogManager.GetLogger(typeof(ScrapEngineContext));

        /// <summary>
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        public AppTopicConfigPathHelper AppTopicPath { get; protected set; }

        /// <summary>
        /// Application config
        /// </summary>
        public ApplicationConfig AppConfig { get; protected set; }

        /// <summary>
        /// The Database class layer
        /// </summary>
        public IScrapDbContext WebDbContext { get; protected set; }

        /// <summary>
        /// Web scrapper html context
        /// </summary>
        public IScrapHtmlContext WebScrapHtml { get; protected set; }

        /// <summary>
        /// The Factory class object used to intiialzie all instances
        /// </summary>
        public ScrapFactory Factory { get; protected set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapEngineContext()
        {
            Factory = new ScrapFactory();
        }

        /// <summary>
        /// Initialize the engine context
        /// </summary>
        /// <param name="appTopic"></param>
        /// <param name="sqldb"></param>
        public void Initialize(AppTopicConfigPathHelper appTopicPath, ApplicationConfig appGenericConfig)
        {
            AppTopicPath = appTopicPath;

            // Find Application Topic specific config
            AppTopicPath.AppTopicMain.AssertExists();

            // Read the application config and set the values by overwriting with the app topic specific config
            ReadApplicationConfig(appGenericConfig);

            // Initialize the database context
            WebDbContext = Factory.CreateDbContext(this);

            // Web scrap html context
            WebScrapHtml = Factory.CreateHtmlContext(this);

            logger.InfoFormat("Web scrap Engine context object which is the starting " +
                "object for a application topic scrapper is successfull initialized for '{0}'", AppTopicPath.AppTopic);
        }

        /// <summary>
        /// A static initializer to construct and initialize the app topic specific scrap context
        /// </summary>
        /// <param name="appTopicPath"></param>
        /// <param name="appGenericConfig"></param>
        /// <param name="genericDbConfig"></param>
        /// <returns></returns>
        public static ScrapEngineContext Init(AppTopicConfigPathHelper appTopicPath, 
            ApplicationConfig appGenericConfig)
        {
            ScrapEngineContext appEngine = new ScrapEngineContext();
            appEngine.Initialize(appTopicPath, appGenericConfig);
            return appEngine;
        }

        /// <summary>
        /// A static initializer to construct and initialize the app topic specific scrap context
        /// </summary>
        /// <param name="appTopicPath"></param>
        /// <param name="appGenericConfig"></param>
        /// <param name="genericDbConfig"></param>
        /// <returns></returns>
        public static void Execute(AppTopicConfigPathHelper appTopicPath,
            ApplicationConfig appGenericConfig)
        {
            logger.InfoFormat("Generate Application Web Scrapped data for {0}", appTopicPath);

            ScrapEngineContext engineContext = ScrapEngineContext.Init(appTopicPath,
                        appGenericConfig);
            engineContext.Run();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// It merges the application generic config to the application topic specific config
        /// </summary>
        /// <param name="appGenericConfig"></param>
        private void ReadApplicationConfig(ApplicationConfig appGenericConfig)
        {
            ApplicationConfig appSpecificConfig = null;
            if (AppTopicPath.AppConfig.Exists)
                appSpecificConfig = DXmlSerializeReader.Load<ApplicationConfig>(AppTopicPath.AppConfig.FullPath);
            
            if (appGenericConfig == null)
                AppConfig = appSpecificConfig;
            else if (appSpecificConfig == null)
                AppConfig = appGenericConfig.Clone();
            else
                AppConfig = appGenericConfig.Union(appSpecificConfig);
        }

        /// <summary>
        /// Execute the main method for generating App Topic specific data
        /// </summary>
        public void Run()
        {
            WebScrapHtml.Run();
        }

        #endregion Methods
    }
}
