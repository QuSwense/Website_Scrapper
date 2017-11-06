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
            
            if (!Parser.Default.ParseArguments(args, options))
                throw new CommandLineException(args, CommandLineException.EErrorType.PARSE_ERROR);

            // Initialize the root config path (Always)
            AppGenericConfigPathHelper.I.Initialize(options.ScrapperFolderPath);
            
            // Check path
            AppGenericConfigPathHelper.I.ScrapperApps.AssertExists();
            AppGenericConfigPathHelper.I.GlobalAppConfig.AssertExists();

            // Read the generic application configuration file independent of any application topic
            ApplicationConfig appGenericConfig = DXmlSerializeReader.Load<ApplicationConfig>(
                AppGenericConfigPathHelper.I.GlobalAppConfig.FullPath);

            // Read database generic config
            DynamicGenericDbConfig genericDbConfig = new DynamicGenericDbConfig();
            genericDbConfig.Read();

            // If application topic value is "*" run scrapper for all
            // available application folders
            if (string.Compare(options.AppTopic, "*", true) == 0)
            {
                // Get a list of all application scrap folders and generate application scrapper context
                foreach (var appTopicPath in AppTopicConfigPathHelper.GetAppTopics())
                {
                    ScrapEngineContext engineContext = ScrapEngineContext.ScrapInitialize(appTopicPath, 
                        appGenericConfig, genericDbConfig);
                    engineContext.Run();
                }
            }
            else
            {
                // For specific application topic value
                ScrapEngineContext engineContext = ScrapEngineContext.ScrapInitialize(new AppTopicConfigPathHelper(options.AppTopic), 
                    appGenericConfig, genericDbConfig);
                engineContext.Run();
            }
        }
    }
}
