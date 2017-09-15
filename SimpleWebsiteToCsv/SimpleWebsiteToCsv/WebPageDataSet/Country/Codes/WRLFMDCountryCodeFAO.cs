using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Country
{
    [Citation(Description = "The purpose of the Network of OIE/FAO FMD Reference Laboratoriesis to make available accurate and timely data to support global surveillance and control of Foot-and-Mouth Disease",
        UrlOnline = "http://www.wrlfmd.org/fmd_serotyping/fmd_cntry_codes.htm")]
    [ReferenceCollection("//table//tr[position() > 1]")]
    public class WRLFMDCountryCodeFAO
    {
        [ReferenceSingle("td[position() = 1]")]
        public string WRLCode { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        [RegexReplace(Pattern = @".* (\(.*)", ReplaceText = "")]
        public string CountryName { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        [RegexReplace(Pattern = @"(.* \().*(\))", ReplaceText = "")]
        public string CountryNameNote { get; set; }
    }
}
