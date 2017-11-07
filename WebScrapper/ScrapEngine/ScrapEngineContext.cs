using ScrapEngine.Bl;
using ScrapEngine.Db;
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
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        public AppTopicConfigPathHelper AppTopicPath { get; protected set; }

        /// <summary>
        /// Application config
        /// </summary>
        public ApplicationConfig AppConfig { get; protected set; }

        /// <summary>
        /// A generic database config
        /// </summary>
        public DynamicGenericDbConfig GenericDbConfig { get; protected set; }

        /// <summary>
        /// The Database class layer
        /// </summary>
        public ScrapDbContext WebDbContext { get; protected set; }

        /// <summary>
        /// Web scrapper html context
        /// </summary>
        public WebScrapHtmlContext WebScrapHtml { get; protected set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapEngineContext() { }

        /// <summary>
        /// Initialize the engine context
        /// </summary>
        /// <param name="appTopic"></param>
        /// <param name="sqldb"></param>
        public void Initialize(AppTopicConfigPathHelper appTopicPath, ApplicationConfig appGenericConfig, DynamicGenericDbConfig genericDbConfig)
        {
            AppTopicPath = appTopicPath;

            // Find Application Topic specific config
            AppTopicPath.AppTopicMain.AssertExists();

            // Read the application config and set the values by overwriting with the app topic specific config
            ReadApplicationConfig(appGenericConfig);

            // Initialize the database context
            WebDbContext = new ScrapDbContext();
            WebDbContext.Initialize(this);

            // Web scrap html context
            WebScrapHtml = new WebScrapHtmlContext();
            WebScrapHtml.Initialize(this);
        }

        /// <summary>
        /// A static initializer to construct and initialize the app topic specific scrap context
        /// </summary>
        /// <param name="appTopicPath"></param>
        /// <param name="appGenericConfig"></param>
        /// <param name="genericDbConfig"></param>
        /// <returns></returns>
        public static ScrapEngineContext ScrapInitialize(AppTopicConfigPathHelper appTopicPath, 
            ApplicationConfig appGenericConfig, DynamicGenericDbConfig genericDbConfig)
        {
            ScrapEngineContext appEngine = new ScrapEngineContext();
            appEngine.Initialize(appTopicPath, appGenericConfig, genericDbConfig);
            return appEngine;
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
            {
                AppConfig = appGenericConfig.Union(appSpecificConfig);
            }
        }

        /// <summary>
        /// Execute
        /// </summary>
        public void Run()
        {
            WebScrapHtml.Run();
        }

        #endregion Methods
    }
}
