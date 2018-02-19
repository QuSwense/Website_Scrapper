using CommandLine;
using ConfigPathHelper;
using log4net;
using ScrapEngine;
using ScrapEngine.Db;
using ScrapEngine.Model;
using ScrapException;
using System;
using WebReader.Xml;

namespace WebScrapper
{
    class Program
    {
        /// <summary>
        /// Log the application data
        /// </summary>
        public static ILog logger = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Pass command line argument for scrapping and saving application data
        /// 1: The name of the application topic, or "*" for all
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program application = new Program();

            logger.Info("Start of the Application Data Scrapper");

            // Get command options
            CommandOptions options = application.ParseCommandLine(args);

            // Initialize
            application.InitializeConfigPath(options);

            // Read the generic application configuration file independent of any application topic
            var appGenericConfig = DXmlSerializeReader.Load<ApplicationConfig>(
                AppGenericConfigPathHelper.I.GlobalAppConfig.FullPath);

            // If application topic value is "*" run scrapper for all
            // available application folders
            if (string.Compare(options.AppTopic, "*", true) == 0)
            {
                logger.Info("Generate Application Web Scrapped data for all App topics present in the config folder");

                // Get a list of all application scrap folders and generate application scrapper context
                foreach (var appTopicPath in AppTopicConfigPathHelper.GetAppTopics())
                    ScrapEngineContext.Execute(appTopicPath, appGenericConfig);
            }
            else
                ScrapEngineContext.Execute(new AppTopicConfigPathHelper(options.AppTopic), appGenericConfig);

            logger.Info("End of the Application Data Scrapper");
        }

        /// <summary>
        /// Parse command line arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CommandOptions ParseCommandLine(string[] args)
        {
            // Parse arguments
            var options = new CommandOptions();

            if (!Parser.Default.ParseArguments(args, options))
                throw new CommandLineException(CommandLineException.EErrorType.PARSE_ERROR, args);

            if (logger.IsDebugEnabled)
                logger.DebugFormat("Command line arguments parsed : \n{0}", string.Join(Environment.NewLine,
                    options.PrintParsed()));

            return options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public void InitializeConfigPath(CommandOptions options)
        {
            // Initialize the root config path at the beginning (Always)
            AppGenericConfigPathHelper.I.Initialize(options.ScrapperFolderPath);

            logger.DebugFormat("Root Config file(s) from the path {0} is initialized",
                AppGenericConfigPathHelper.I.RootPath);

            // Check mandatory path
            AppGenericConfigPathHelper.I.GlobalAppConfig.AssertExists();

            logger.DebugFormat("Global Application config path {0} is present",
                AppGenericConfigPathHelper.I.GlobalAppConfig.FullPath);

            // Read database generic config
            DynamicGenericDbConfig.I.Read();

            // Before starting the loops check the global database configs
            AppGenericConfigPathHelper.I.DbScriptsTableMdt.AssertExists();
            AppGenericConfigPathHelper.I.DbScriptsTableScrapMdt.AssertExists();
            AppGenericConfigPathHelper.I.DbScriptsPerformanceMdt.AssertExists();
            AppGenericConfigPathHelper.I.DbScriptsColumnScrapMdt.AssertExists();
        }
    }
}
