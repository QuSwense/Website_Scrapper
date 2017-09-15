using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Country
{
    [QSReference("IOC uses three-letter abbreviations (3LA) to recognize the various NOCs in its organization. It has also been known that several of the NOCs (National Olympic Committee) have had their 3LA change within the past decade.")]
    [QSURIDataSource(Online = "https://en.wikipedia.org/wiki/List_of_IOC_country_codes")]
    [ReferenceCollection("(//div[@id='mw-content-text']/div[@class='mw-parser-output']/table)[1]//table[@class='wikitable']//tr[position() > 1]")]
    public class IOCMemberCode
    {
        [ReferenceSingle("td[position() = 1]")]
        public string Code { get; set; }

        [ReferenceSingle("td[position() = 2]/a", Attribute = "href")]
        [RegexReplace(Pattern = @"https://web.archive.org/web/\d+/", ReplaceText = "")]
        public string NationalOlympicCommitteeUrl { get; set; }

        [ReferenceSingle("td[position() = 3]/a")]
        public string Name { get; set; }

        [ReferenceSingle("td[position() = 3]/a", Attribute = "href")]
        public string WikiOlympicsUrl { get; set; }
    }
}
