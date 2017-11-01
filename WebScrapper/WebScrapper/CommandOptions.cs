using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper
{
    public class CommandOptions
    {
        [Option("p", DefaultValue=".", HelpText="Scrapper Apps Main Folder Path")]
        public string ScrapperFolderPath { get; set; }

        [Option("a", DefaultValue = "*", HelpText = "Scrapper Application Topic")]
        public string AppTopic { get; set; }
    }
}
