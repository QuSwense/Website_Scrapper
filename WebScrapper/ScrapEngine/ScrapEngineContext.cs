using DynamicDatabase;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Model;
using DynamicDatabase.Scrap;
using ScrapEngine.Bl;
using ScrapEngine.Db;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebCommon.Config;
using WebReader.Csv;
using WebScrapper.Db.Ctx;

namespace ScrapEngine
{
    /// <summary>
    /// The main controller class which controls the web scrapping of web page data from
    /// the websites and stores in the database. The Scrapping rule configuration is in
    /// the config xml file.
    /// </summary>
    public class ScrapEngineContext : IScrapEngineContext
    {
        /// <summary>
        /// The folder path for ScrapperApps. If empty assume current
        /// </summary>
        public string ScrapperFolderPath { get; protected set; }

        /// <summary>
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        public string AppTopic { get; protected set; }

        /// <summary>
        /// Application config
        /// </summary>
        public ApplicationConfig AppConfig { get; protected set; }

        /// <summary>
        /// A generic database config
        /// </summary>
        public DynamicDbConfig GenericDbConfig { get; protected set; }

        /// <summary>
        /// The Database class layer
        /// </summary>
        public ScrapDbContext WebDbContext { get; protected set; }

        /// <summary>
        /// Web scrapper html context
        /// </summary>
        public WebScrapHtmlContext WebScrapHtml { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapEngineContext() { }

        /// <summary>
        /// Initialize the engine context
        /// </summary>
        /// <param name="appTopic"></param>
        /// <param name="sqldb"></param>
        public void Initialize(string folderPath, string appTopic, ApplicationConfig appGenericConfig, DynamicDbConfig genericDbConfig)
        {
            ScrapperFolderPath = folderPath;
            AppTopic = appTopic;
            
            // Read the application config and set the values by overwriting with the app topic specific config
            ReadApplicationConfig(appGenericConfig);

            WebDbContext = new ScrapDbContext(this);
            WebDbContext.Initialize();

            WebScrapHtml = new WebScrapHtmlContext();
            WebScrapHtml.Initialize();
            WebScrapHtml.Run();
        }

        /// <summary>
        /// It merges the application generic config to the application topic specific config
        /// </summary>
        /// <param name="appGenericConfig"></param>
        private void ReadApplicationConfig(ApplicationConfig appGenericConfig)
        {
            // Find Application Topic specific config
            ApplicationConfig appSpecificConfig = null;
            var appSpecificConfigFile = ConfigPathHelper.GetAppConfigPath(ScrapperFolderPath, AppTopic);

            if(File.Exists(appSpecificConfigFile))
                using (CSVReader reader = new CSVReader(appSpecificConfigFile, appSpecificConfig)) reader.Read();

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
    }
}
