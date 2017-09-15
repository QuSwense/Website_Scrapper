using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [Citation(UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\The World Factbook - AirportsCount.html",
      UrlOnline = @"https://www.cia.gov/library/publications/the-world-factbook/rankorder/2053rank.html")]
    [ReferenceCollection(XPath = "//div[@id='mw-pages']/div[@class='mw-content-ltr']//div[@class='mw-category-group']/ul/li")]
    public class WikiAirportsByCountryUrlData
    {
        [ReferenceSingle(XPath = "a")]
        public string CountryName { get; set; }

        [ReferenceSingle(XPath = "a", Attribute = "href")]
        public string AirportURL { get; set; }
    }
}
