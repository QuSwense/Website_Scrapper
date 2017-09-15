using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.CityMapHq
{
    [Citation(LogoOnlineUrl = "http://www.citymaphq.com/images/citymap.gif")]
    [ReferenceCollection("//table[@class='layout codelist']//tr[position() > 1]")]
    public class CMHCode<T>
    {
        [ReferenceSingle("td[position() = 1]/a")]
        public string RegionName { get; set; }

        [ReferenceSingle("td[position() = 1]/a", Attribute = "href")]
        public string RegionURL { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        [RegexReplace(Pattern = "^-$")]
        [StringSplit()]
        public List<T> Code { get; set; }
    }
}
