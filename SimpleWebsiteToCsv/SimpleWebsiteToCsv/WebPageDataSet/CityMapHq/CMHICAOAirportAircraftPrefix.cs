using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.CityMapHq
{
    [QSReferenceFromDataSource("//div[@class='padtop padbottom padright']/div/p[position() = 1]")]
    [QSURIDataSource(Online = "http://www.citymaphq.com/codes/icao.html")]
    [QSReferenceField("AircraftCode", Text = "ICAO Aircraft Code")]
    public class CMHICAOAirportAircraftPrefix : CMHCode<string>
    {
        [ReferenceSingle("td[position() = 3]")]
        [StringSplit()]
        public List<string> AircraftCode { get; set; }
    }
}
