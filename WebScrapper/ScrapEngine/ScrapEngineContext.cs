using DynamicDatabase;
using DynamicDatabase.Scrap;
using ScrapEngine.Bl;
using ScrapEngine.Db;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
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
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        public string AppTopic { get; protected set; }

        /// <summary>
        /// Application config
        /// </summary>
        public ApplicationConfig AppConfig { get; protected set; }

        /// <summary>
        /// Read the Database configuration
        /// </summary>
        public DynamicDbConfig MetaDbConfig { get; protected set; }

        /// <summary>
        /// The configuration for web scrapping
        /// </summary>
        public WebScrapDbContext WebScrapDb { get; protected set; }

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
        public void Initialize(string appTopic, string sqldb)
        {
            AppTopic = appTopic;

            using (CSVReader reader = new CSVReader(ConfigPathHelper.GetAppConfigPath(AppTopic),
                    AppConfig))
            {
                reader.Read();
            }

            MetaDbConfig = new DynamicDbConfig(AppTopic);
            MetaDbConfig.Initialize();
            MetaDbConfig.Read();

            if (string.Compare(AppConfig.Db(), "sqlite", true) == 0)
                DynamicDbFactory.RegisterSqlite();
            else
                throw new Exception("Unknwon database. Not Supported.");

            WebScrapDb = new WebScrapDbContext();
            WebScrapDb.Initialize(ConfigPathHelper.GetDbFilePath(AppTopic), AppTopic);
            if (AppConfig.DoCreateDb())
                WebScrapDb.CreateDatabase();
            WebScrapDb.Open();

            if(AppConfig.DoCreateDb())
                WebScrapDb.CreateTable(MetaDbConfig.TableColumnConfigs);

            WebScrapHtml = new WebScrapHtmlContext();
            WebScrapHtml.Initialize();
            WebScrapHtml.Run();
            WebScrapDb.Close();
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
