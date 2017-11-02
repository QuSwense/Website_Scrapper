using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
