using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.config;
using WebScrapper.db;

namespace WebScrapper.scrap
{
    public class WebScrapperFactory
    {
        public static WebScrapperBL GetScrapper(string appFolder, DbGeneratorBL DbGenerator,
            AppConfig Config, ScrapWebDataConfig ScrapConfig)
        {
            switch(appFolder)
            {
                default:
                    return new WebScrapperBL(appFolder, DbGenerator, Config, ScrapConfig);
            }
        }
    }
}
