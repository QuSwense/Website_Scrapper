using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.FAO
{
    [Citation(LogoOnlineUrl = "http://www.fao.org/fileadmin/templates/faoweb/images/FAO-logo.png", Copyright = "FAO of the UN")]
    [QSReferenceFromDataSource(OnlineUrl = "http://www.fao.org/about/en/", XPath = "//div[@class='csc-textpic-text']/p[position() = 1]")]
    [QSReferenceFromDataSource(OnlineUrl = "http://www.fao.org/countryprofiles/en/", XPath = "//div[@id='c467418']/p")]
    [QSURIDataSource(Online = "http://www.fao.org/countryprofiles/iso3list/en/")]
    [ReferenceCollection("//div[@class='tx-dynalist-pi1']/table//tr[position() > 1]")]
    public class FAOMemberCountryCode
    {
        [ReferenceSingle("td[position() = 1]/a")]
        public string ShortName { get; set; }

        [ReferenceSingle("td[position() = 1]/a", Attribute = "href")]
        public string FAOCountryProfileURL { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string OfficialName { get; set; }

        [ReferenceSingle("td[position() = 6]")]
        public string UNDPCode { get; set; }

        [ReferenceSingle("td[position() = 7]")]
        public string FOSTATCode { get; set; }

        [ReferenceSingle("td[position() = 8]")]
        public int GAULCode { get; set; }

        [QSUriRTTIDataSource("", "FAOCountryProfileURL")]
        
        public KeyStatisticsFAO CountryStat { get; set; }
    }

    [ReferenceSingle("//aside[@id='rightcolumn']//div[@id='statisticsbox']")]
    public class KeyStatisticsFAO
    {
        [ReferenceSingle(".//div[@id='countryArea']/div[@class='statsValue']")]
        public string CountryArea1000HA { get; set; }

        [ReferenceSingle(".//div[@id='countryArea']/div[@class='statsSource']")]
        public string SourceCountryArea1000HA { get; set; }

        [ReferenceSingle(".//div[@id='landArea']/div[@class='statsValue']")]
        public string LandArea1000HA { get; set; }

        [ReferenceSingle(".//div[@id='landArea']/div[@class='statsSource']")]
        public string SourceLandArea1000HA { get; set; }

        [ReferenceSingle(".//div[@id='agriculturalArea']/div[@class='statsValue']")]
        public string AgricultureArea1000HA { get; set; }

        [ReferenceSingle(".//div[@id='agriculturalArea']/div[@class='statsSource']")]
        public string SourceAgricultureArea1000HA { get; set; }

        [ReferenceSingle(".//div[@id='forest']/div[@class='statsValue']")]
        public string ForestArea1000HA { get; set; }

        [ReferenceSingle(".//div[@id='forest']/div[@class='statsSource']")]
        public string SourceForestArea1000HA { get; set; }
    }
}
