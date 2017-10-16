using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.country;

namespace WebScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Execute an engine
            AppEngine appEngine = new AppEngine("country");
            appEngine.Run();
        }
    }
}
