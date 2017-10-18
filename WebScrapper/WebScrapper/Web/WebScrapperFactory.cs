using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebScrapper.Config;
using WebScrapper.Db;
using WebScrapper.Web.Config;

namespace WebScrapper.Web
{
    public class WebScrapperFactory
    {
        public static WebScrapperBL GetScrapper(string appFolder, DbGeneratorBL DbGenerator,
            ApplicationConfig Config, WebDataConfig ScrapConfig)
        {
            switch (appFolder)
            {
                default:
                    return new WebScrapperBL(appFolder, DbGenerator, Config, ScrapConfig);
            }
        }
    }
}
