using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Country
{
    [QSURIFileDataSource(Online = "http://www.arrs.net/IAAF_CC2.htm", XPath ="//body/pre/font[position() = 1]")]
    [QSReferenceFromDataSource("//body/p[position() = 1]")]
    [StringSplit("   ")]
    public class IAAFCountryCode
    {
        [QSIndex(0)]
        public string Code { get; set; }

        [QSIndex(1)]
        public string Region { get; set; }
    }
}
