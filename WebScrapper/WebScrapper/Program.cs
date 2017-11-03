using CommandLine;
using ScrapEngine;
using ScrapEngine.Db;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebCommon.Error;
using WebCommon.PathHelp;
using WebReader.Csv;
using WebReader.Xml;

namespace WebScrapper
{
    class Program
    {
        /// <summary>
        /// Pass argument for generating application data
        /// 1: The name of the application, or "*" for all
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Parse arguments
            CommandOptions options = new CommandOptions();
            var result = Parser.Default.ParseArguments(args, options);

            if(result == false)
            {
                Console.WriteLine("Command line arguments error");
                return;
            }

            // Initialize the root config path (Always)
            AppGenericConfigPathHelper.I.Initialize(options.ScrapperFolderPath);

            // Check path
            if (!AppGenericConfigPathHelper.I.GlobalConfig.Exists)
                throw new PathException(AppGenericConfigPathHelper.I.GlobalConfig.FullPath,
                    PathException.EErrorType.NOT_EXISTS);

            // Read the generic application configuration file independent of any application topic
            ApplicationConfig AppConfig = DXmlSerializeReader.Load<ApplicationConfig>(
                AppGenericConfigPathHelper.I.GlobalConfig.FullPath);

            using (CSVReader reader = new CSVReader(AppGenericConfigPathHelper.I.GlobalConfig.FullPath, AppConfig)) reader.Read();

            // Read database generic config
            DynamicDbConfig GenericDbConfig = new DynamicDbConfig(options.ScrapperFolderPath);
            GenericDbConfig.Read();

            // If application topic value is "*" run scrapper for all
            // available application folders
            if (string.Compare(options.AppTopic, "*", true) == 0)
            {
                var scrapperAppFolder = ConfigPathHelper.GetScrapperAppFolderPath(options.ScrapperFolderPath);

                if(!Directory.Exists(scrapperAppFolder))
                {
                    Console.WriteLine("No folder for Scrapper Apps found : " + scrapperAppFolder);
                    return;
                }

                // Get a list of all application scrap folders and generate application scrapper context
                foreach (var folderPath in Directory.GetDirectories(scrapperAppFolder, "App*"))
                {
                    DirectoryInfo dinfo = new DirectoryInfo(folderPath);
                    ScrapEngineContext appEngine = new ScrapEngineContext();
                    appEngine.Initialize(options.ScrapperFolderPath, dinfo.Name, AppConfig, GenericDbConfig);
                }
            }
            else
            {
                // For specific application topic value
                ScrapEngineContext appEngine = new ScrapEngineContext();
                appEngine.Initialize(options.ScrapperFolderPath, options.AppTopic, AppConfig, GenericDbConfig);
            }
        }
    }
}
