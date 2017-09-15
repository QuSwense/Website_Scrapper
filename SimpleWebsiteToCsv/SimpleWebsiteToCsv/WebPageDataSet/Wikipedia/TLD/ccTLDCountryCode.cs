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
    [QSURIDataSource(Online = "https://en.wikipedia.org/wiki/List_of_Internet_top-level_domains")]
    public class ccTLDCountryCode
    {
        public List<ccTLDCountryNormalCode> NormalCodes { get; set; }
        public List<ccTLDCountryInternationalisedCode> InternationalisedCodes { get; set; }
    }

    [ReferenceCollection("(//div[@id='mw-content-text']/div[@class='mw-parser-output']/table)[5]//table[@class='wikitable']//tr[position() > 1]")]
    public class ccTLDCountryNormalCode
    {
        [ReferenceSingle("td[position() = 1]")]
        public string Code { get; set; }

        [ReferenceSingle("td[position() = 2]/a")]
        public string Region { get; set; }

        [ReferenceSingle("td[position() = 3]")]
        public string CodeExplanation { get; set; }

        [ReferenceSingle("td[position() = 4]")]
        public string Notes { get; set; }

        [ReferenceSingle("td[position() = 5]")]
        [BoolConversion(TruthValues = "Yes")]
        public bool SupportsIDN { get; set; }

        [ReferenceSingle("td[position() = 6]")]
        [BoolConversion(TruthValues = "Yes")]
        public bool PresenceOfDSForDNSSEC { get; set; }

        [ReferenceSingle("td[position() = 7]")]
        [BoolConversion(TruthValues = "Yes")]
        public bool AllowSLD { get; set; }

        [ReferenceSingle("td[position() = 8]")]
        [BoolConversion(TruthValues = "Yes")]
        public bool SupportsIPv6 { get; set; }
    }

    [ReferenceCollection("(//div[@id='mw-content-text']/div[@class='mw-parser-output']/table)[6]//table[@class='wikitable']//tr[position() > 1]")]
    public class ccTLDCountryInternationalisedCode
    {
        [ReferenceSingle("td[position() = 1]")]
        public string DNSName { get; set; }

        [ReferenceSingle("td[position() = 2]/a", Attribute = "title")]
        public string IDNccTLD { get; set; }

        [ReferenceSingle("td[position() = 2]/a", Attribute = "href")]
        public string IDNccTLDURL { get; set; }

        [ReferenceSingle("td[position() = 3]")]
        public string CountryName { get; set; }

        [ReferenceSingle("td[position() = 4]")]
        public string Language { get; set; }

        [ReferenceSingle("td[position() = 5]")]
        public string Script { get; set; }

        [ReferenceSingle("td[position() = 6]")]
        public string Transliteration { get; set; }

        [ReferenceSingle("td[position() = 7]")]
        public string Comments { get; set; }

        [ReferenceSingle("td[position() = 9]")]
        [BoolConversion(TruthValues = "Yes")]
        public bool PresenceOfDSForDNSSEC { get; set; }
    }
}
