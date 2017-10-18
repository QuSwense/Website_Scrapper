using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Config;
using WebScrapper.Db;
using WebScrapper.Web;
using WebScrapper.Web.Config;

namespace WebScrapper
{
    /// <summary>
    /// The application engine class.
    /// It represents one single application class at a time.
    /// </summary>
    public class AppEngine
    {
        /// <summary>
        /// The name of the application. It is same as the folder under the "App" directory.
        /// </summary>
        public string AppTopic { get; protected set; }

        /// <summary>
        /// Read the application name specific Config.xml file
        /// </summary>
        public ApplicationConfig Config { get; protected set; }

        /// <summary>
        /// Database config model. It contains the list of config files related to database
        /// </summary>
        public DbConfigModel DbConfig { get; protected set; }

        /// <summary>
        /// A class which helps generating and maipulating application database
        /// </summary>
        public DbGeneratorBL DbGenerator { get; protected set; }

        /// <summary>
        /// A web scrapper class
        /// </summary>
        public WebScrapperBL WebScrapper { get; protected set; }

        /// <summary>
        /// A web scrapper config model
        /// </summary>
        public WebDataConfig ScrapConfig { get; protected set; }

        /// <summary>
        /// COnstructor with parameters
        /// </summary>
        /// <param name="appFolder"></param>
        public AppEngine(string appFolder)
        {
            AppTopic = appFolder;
        }

        /// <summary>
        /// Execute the application engine
        /// </summary>
        public void Run()
        {
            // Read Config
            Config = ConfigHelper.Read<ApplicationConfig>(ConfigHelper.GetAppConfigPath(AppTopic));

            // Db config
            DbConfig = new DbConfigModel(AppTopic);
            DbConfig.Read();

            // Db Generator
            DbGenerator = DbFactory.GetGenerator(AppTopic, DbConfig);
            DbGenerator.Generate();
            DbGenerator.OpenConnection();

            // Web scrapper
            ScrapConfig = ConfigHelper.Read<WebDataConfig>(ConfigHelper.GetScrapConfigPath(AppTopic));
            WebScrapper = WebScrapperFactory.GetScrapper(AppTopic, DbGenerator, Config, ScrapConfig);
            WebScrapper.Run();
            DbGenerator.CloseConnection();
        }
    }
}
