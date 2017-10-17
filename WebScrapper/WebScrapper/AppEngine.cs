using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Config;
using WebScrapper.Db;
using WebScrapper.Web;

namespace WebScrapper
{
    /// <summary>
    /// The application engine class
    /// </summary>
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
            Config = ConfigHelper.Read<AppConfig>(ConfigHelper.GetAppConfigPath(AppTopic));

            // Db config
            DbConfig = new DbConfigModel(AppTopic);
            DbConfig.Read();

            // Db Generator
            DbGenerator = DbFactory.GetGenerator(AppTopic, DbConfig);
            DbGenerator.Generate();

            // Web scrapper
            ScrapConfig = ConfigHelper.Read<ScrapWebDataConfig>(ConfigHelper.GetScrapConfigPath(AppTopic));
            WebScrapper = WebScrapperFactory.GetScrapper(AppTopic, DbGenerator, Config, ScrapConfig);
            WebScrapper.Run();
        }
    }
}
