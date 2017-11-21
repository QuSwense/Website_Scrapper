using CommandLine;
using System.Collections.Generic;

namespace WebScrapper
{
    /// <summary>
    /// The command line options
    /// </summary>
    public class CommandOptions
    {
        /// <summary>
        /// The path of the root scrapper folder which contains all the application
        /// topics for web scrapping
        /// </summary>
        [Option("p", DefaultValue=".", HelpText="Scrapper Apps Main Folder Path")]
        public string ScrapperFolderPath { get; set; }

        /// <summary>
        /// The application topic for web scrapper
        /// </summary>
        [Option("a", DefaultValue = "*", HelpText = "Scrapper Application Topic")]
        public string AppTopic { get; set; }

        /// <summary>
        /// Get the list of Key = value pairs
        /// </summary>
        /// <returns></returns>
        public string[] PrintParsed()
        {
            return new string[]
            {
                string.Format("ScrapperFolderPath = {0}", ScrapperFolderPath),
                string.Format("AppTopic = {0}", AppTopic)
            };
        }
    }
}
