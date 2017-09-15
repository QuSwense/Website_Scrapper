using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Wiki
{
    [QSReferenceFromDataSource(XPath = "//div[@class='mw-parser-output']/p[position() = 1]")]
    [QSURIDataSource(Online = "https://en.wikipedia.org/wiki/List_of_CGF_country_codes")]
    [ReferenceCollection("//table[@class='wikitable' and position() < 27]//tr[position() > 1]")]
    public class CommonwealthGamesCountryCode
    {
        [ReferenceCollection("td[position() = 1]")]
        public string Code { get; set; }

        [ReferenceCollection("td[position() = 2]")]
        public string Nation { get; set; }
    }
}
