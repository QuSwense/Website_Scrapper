using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Country.Geography
{
    [Citation(Description = "The GeoNames geographical database covers all countries and contains over eleven million placenames that are available for download free of charge.", UrlOnline = "http://www.geonames.org/countries/")]
    [ReferenceCollection("//table[@class='restable sortable']//tr[position() > 1]")]
    public class GeoNamesCountriesData
    {
        [ReferenceSingle("td[position() = 1]")]
        public string ISO3166A2Code { get; set; }

        [ReferenceSingle("td[position() = 4]")]
        public string FIPSCode { get; set; }

        [ReferenceSingle("td[position() = 5]/a")]
        public string CountryName { get; set; }

        [ReferenceSingle("td[position() = 5]/a", Attribute ="href")]
        public string CountryURL { get; set; }

        [ReferenceSingle("td[position() = 6]")]
        public string CapitalMain { get; set; }

        [CitationRegex(PropertyName = "CountryURL")]
        [ReferenceSingle("//td[contains(text(), 'country name :')]/../td[position() = 2]/a[contains(text(), 'other languages')]",
            Attribute = "href")]
        public string OtherNamesUrl { get; set; }

        [CitationRegex(PropertyName = "CountryURL")]
        [ReferenceSingle("//td[contains(text(), 'languages :')]/../td[position() = 2]")]
        [StringSplit(",")]
        public List<GeoNameLanguage> CommonLanguages { get; set; }

        public CountryOtherLanguages OtherNames { get; set; }
    }

    [CitationRegex(PropertyName = "OtherNamesUrl")]
    [ReferenceCollection("//table[@class='restable sortable']//tr[position() > 1]")]
    public class CountryOtherLanguages
    {
        [ReferenceSingle("td[position() = 1]")]
        public string Name { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string LanguageName { get; set; }

        [ReferenceSingle("td[position() = 3]")]
        public string LanguageCode { get; set; }
    }

    [StringSplit("()")]
    public class GeoNameLanguage
    {
        [QSIndex(0)]
        public string Name { get; set; }

        [QSIndex(1)]
        public string Code { get; set; }
    }
}
