using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.ITU
{
    [QSURIDataSource(Online = "https://www.itu.int/en/ITU-R/terrestrial/fmd/Pages/geo_area_list.aspx")]
    [QSReference("The ITU Radiocommunication Sector (ITU-R) plays a vital role in the global management of the radio-frequency spectrum and satellite orbits - limited natural resources which are increasingly in demand from a large and growing number of services such as fixed, mobile, broadcasting, amateur, space research, emergency telecommunications, meteorology, global positioning systems, environmental monitoring and communication services")]
    [ReferenceCollection("//table[@class='table table-striped table-condensed']//tr[position() > 1]")]
    public class ITURegionCode
    {
        [ReferenceSingle("td[position() = 1]")]
        public string Code { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string Country { get; set; }
    }
}
