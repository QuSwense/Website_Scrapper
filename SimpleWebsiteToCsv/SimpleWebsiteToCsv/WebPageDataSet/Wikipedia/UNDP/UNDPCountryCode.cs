using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Country
{
    [QSReference("The United Nations Development Programme (UNDP) is the United Nations' global development network. This is the list of UNDP country codes.")]
    [QSURIDataSource(Online = "https://en.wikipedia.org/wiki/List_of_UNDP_country_codes")]
    [ReferenceCollection("//table[@class='wikitable']//tr[position() > 1]")]
    public class UNDPCountryCode
    {
        [ReferenceSingle("td[position() > 1]")]
        public string Code { get; set; }

        [ReferenceSingle("td[position() > 2]")]
        public string Entity { get; set; }

        [ReferenceSingle("td[position() > 3]")]
        public string Comment { get; set; }
    }
}
