using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.ITU
{
    [QSReference("The International Telecommunication Union (ITU) allocates call sign prefixes for radio and television stations of all types. They also form the basis for, but do not exactly match, aircraft registration identifiers. These prefixes are agreed upon internationally, and are a form of country code. A call sign can be any number of letters and numerals but each country must only use call signs that begin with the characters allocated for use in that country.")]
    [QSURIDataSource(Online = "https://www.itu.int/en/ITU-R/terrestrial/fmd/Pages/mid.aspx")]
    [ReferenceCollection("//table[@class='table table-striped table-condensed']//tr[position() > 1]")]
    public class ITUCallSignPrefixes
    {
        [ReferenceCollection("td[position() = 1]/span")]
        public List<string> CodeRanges { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string Country { get; set; }
    }
}
