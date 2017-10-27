using ScrapEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            if (args != null && args.Length > 0 && args[0] == "*")
            {
                // Get a list of all app folders
                foreach (var folderPath in Directory.GetDirectories("App"))
                {
                    ScrapEngineContext appEngine = new ScrapEngineContext();
                    appEngine.Initialize(folderPath, "sqlite");
                }
            }
            else
            {
                ScrapEngineContext appEngine = new ScrapEngineContext();
                appEngine.Initialize("country", "sqlite");
            }
        }
    }
}
