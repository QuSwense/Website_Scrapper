using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Wiki
{
    [QSReference("ISO 3166 is the International Standard for country codes and codes for their subdivisions published by the International Organization for Standardization (ISO). To be considered, these areas cannot be an exclave of the parent country nor an island within the territorial waters of the parent country, and must be physically separated from its parent country. It is referred as 'Codes for the representation of names of countries and their subdivisions'.")]
    [QSURIDataSource(Online = "https://en.wikipedia.org/wiki/ISO_3166-1")]
    [ReferenceCollection(XPath = "(//table[@class='wikitable sortable jquery-tablesorter'])[2]//tr[position() > 1]")]
    public class WikiISO3166Alpha1
    {
        [ReferenceSingle(XPath = "td[position() = 1]")]
        [About(Description = "The names of countries or areas refer to their short form used in day-to-day operations of the United Nations and not necessarily to their official name as used in formal documents. These names are based on the United Nations Terminology Database (UNTERM).")]
        public string RegionShortName { get; set; }

        [ReferenceSingle(XPath = "td[position() = 2]")]
        [About(Description = "Alpha-2 code – a two-letter code that represents a country name, recommended as the general purpose code")]
        public string Alpha2Code { get; set; }

        [ReferenceSingle(XPath = "td[position() = 3]")]
        [About(Description = "Alpha-3 code – a three-letter code that represents a country name, which is usually more closely related to the country name")]
        public string Alpha3Code { get; set; }

        [ReferenceSingle(XPath = "td[position() = 4]")]
        [About(Description = "Three-digit country codes which are identical to those developed and maintained by the United Nations Statistics Division")]
        public string NumericCode { get; set; }
    }
}
