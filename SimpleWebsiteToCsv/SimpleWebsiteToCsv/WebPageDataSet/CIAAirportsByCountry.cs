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
    [ReferenceCollection(XPath = "//div[@class='table-responsive double-scroll']/table//tr[position() > 1]")]
    public class CIAAirportsByCountry
    {
        [ReferenceSingle(XPath = "td[position() = 1]")]
        public string Rank { get; set; }

        [ReferenceSingle(XPath = "td[position() = 2]/a")]
        public string CountryName { get; set; }

        [ReferenceSingle(XPath = "td[position() = 3]")]
        public string Count { get; set; }

        [ReferenceSingle(XPath = "td[position() = 4]")]
        public string DateOfInformation { get; set; }
    }
}
