using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.ITU
{
    [QSReferenceFromDataSource("//div[@class='ms-rtestate-field']/div[position() = 1]/div[position() = 1]",
        OnlineUrl = "https://www.itu.int/en/membership/Pages/overview.aspx")]
    [QSURIDataSource(Online = "https://www.itu.int/online/mm/scripts/gensel8")]
    [ReferenceCollection("//div[@class='container']/div[@class='content']/table//tr[position() > 1]")]
    public class ITUMemberStates
    {
        [ReferenceSingle("td[position() = 1]/a")]
        public string Name { get; set; }

        [ReferenceSingle("td[position() = 1]/a", Attribute ="href")]
        public string NameURL { get; set; }

        /// <summary>
        /// Region A	-	The Americas
        /// Region B	-	Western Europe
        /// Region C	-	Eastern Europe and Northern Asia
        /// Region D	-	Africa
        /// Region E	-	Asia and Australasia
        /// </summary>
        [ReferenceSingle("td[position() = 2]")]
        public string AdministrativeRegion { get; set; }

        [ReferenceSingle("td[position() = 3]")]
        public string CountrySymbol { get; set; }

        [ReferenceSingle("td[position() = 4]")]
        public string DomainName { get; set; }

        [ReferenceSingle("td[position() = 5]")]
        [QSFormatFieldDateTime("yyyy/M/dd")]
        public DateTime DateOfEntry { get; set; }
    }
}
