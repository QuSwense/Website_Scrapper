using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Wiki
{
    [QSReferenceFromDataSource(XPath = "//div[@id='mw-content-text']/div[position() = 1]/p[position() = 1]")]
    [QSURIDataSource(Online = "https://en.wikipedia.org/wiki/List_of_country_calling_codes")]
    public class ISDCodeData
    {
        public ISDCountryRegion CountryOrRegion { get; set; }
        public ISDNonCountryLocation NonCountry { get; set; }
    }

    [ReferenceCollection("(//div[@id='mw-content-text']/div[@class='mw-parser-output']/table)[2]//tr[position() > 1]")]
    public class ISDCountryRegion
    {
        [ReferenceSingle("td[position() = 1]")]
        public string CountryOrTerritory { get; set; }

        [ReferenceCollection("td[position() = 2]/a")]
        public List<ISDCode> ISDCodes { get; set; }
    }

    public class ISDCode
    {
        [ReferenceSingle(Attribute = "title")]
        public string Code { get; set; }

        [ReferenceSingle(Attribute = "href")]
        public string CodeUrl { get; set; }
    }

    [ReferenceCollection("(//div[@id='mw-content-text']/div[@class='mw-parser-output']/table)[3]//tr[position() > 1]")]
    public class ISDNonCountryLocation
    {
        [ReferenceSingle("td[position() = 1]")]
        public string BaseName { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string CallingCode { get; set; }

        [ReferenceSingle("td[position() = 3]/a")]
        public string Country { get; set; }

        [ReferenceSingle("td[position() = 4]")]
        [BoolConversion(TruthValues = "Can be direct dialed")]
        public bool CanBeDirectDialled { get; set; }
    }
}
