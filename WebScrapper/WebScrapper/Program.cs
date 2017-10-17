using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper
{
    class Program
    {
        /// <summary>
        /// Pass argument for generating
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            AppEngine appEngine = new AppEngine(args[0]);
            appEngine.Run();
        }
    }
}
