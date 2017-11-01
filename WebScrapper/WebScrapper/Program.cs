using CommandLine;
using ScrapEngine;
using ScrapEngine.Db;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebCommon.Config;
using WebReader.Csv;

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

            // Read the generic application configuration file independent of any application topic
            ApplicationConfig AppConfig = null;
            string appGenericConfigPath = ConfigPathHelper.GetAppGenericConfigPath(options.ScrapperFolderPath);

            DynamicDbConfig GenericDbConfig = new DynamicDbConfig(options.ScrapperFolderPath);
            GenericDbConfig.Read();

            if (File.Exists(appGenericConfigPath))
                using (CSVReader reader = new CSVReader(appGenericConfigPath, AppConfig)) reader.Read();

            if (string.Compare(options.AppTopic, "*", true) == 0)
            {
                var scrapperAppFolder = ConfigPathHelper.GetScrapperAppFolderPath(options.ScrapperFolderPath);

                if(!Directory.Exists(scrapperAppFolder))
                {
                    Console.WriteLine("No folder for Scrapper Apps found : " + scrapperAppFolder);
                    return;
                }

                // Get a list of all app folders and generate application scrapper databases for all
                foreach (var folderPath in Directory.GetDirectories(scrapperAppFolder, "App*"))
                {
                    DirectoryInfo dinfo = new DirectoryInfo(folderPath);
                    ScrapEngineContext appEngine = new ScrapEngineContext();
                    appEngine.Initialize(options.ScrapperFolderPath, dinfo.Name, AppConfig, GenericDbConfig);
                }
            }
            else
            {
                ScrapEngineContext appEngine = new ScrapEngineContext();
                appEngine.Initialize(options.ScrapperFolderPath, options.AppTopic, AppConfig, GenericDbConfig);
            }
        }
    }
}
