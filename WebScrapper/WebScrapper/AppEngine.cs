using WebScrapper.config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebScrapper.db;
using WebScrapper.scrap;
using WebScrapper.common;

namespace WebScrapper.country
{
    public class AppEngine
    {
        public string AppTopic { get; protected set; }
        public AppConfig Config { get; protected set; }
        public DbConfigModel DbConfig { get; protected set; }
        public DbGeneratorBL DbGenerator { get; protected set; }
        public WebScrapperBL WebScrapper { get; protected set; }
        public ScrapWebDataConfig ScrapConfig { get; protected set; }

        public AppEngine(string appFolder)
        {
            AppTopic = appFolder;
        }

        public void Run()
        {
            // Read Config
            Config = ConfigHelper.Read<AppConfig>(Path.Combine(AppTopic, AppTopic + "Config.xml"));

            // Db config
            DbConfig = new DbConfigModel(AppTopic);
            DbConfig.Read();

            // Db Generator
            DbGenerator = DbFactory.GetGenerator(AppTopic, DbConfig);
            DbGenerator.Generate();

            // Web scrapper
            ScrapConfig = ConfigHelper.Read<ScrapWebDataConfig>(Path.Combine(AppTopic, "scrap", AppTopic + "Scrap.xml"));
            WebScrapper = WebScrapperFactory.GetScrapper(AppTopic, DbGenerator, Config, ScrapConfig);
            WebScrapper.Run();
        }
    }
}
