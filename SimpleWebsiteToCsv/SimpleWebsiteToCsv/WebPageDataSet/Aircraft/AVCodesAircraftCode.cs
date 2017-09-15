using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Aircraft
{
    [Citation(Description = "A list of IATA Aircraft Type Codes with ICAO tie-ups used in Airline Computer Reservation Systems, Timetables, Airport Information Systems and Schedule Data Publications. e.g OAG Airline Guides and Pocket Guides.",
        UrlOnline = "http://www.avcodes.co.uk/acrtypes.asp")]
    [ReferenceCollection("//div[@align='center']//table[@class='ink-table alternating']//tr[position() > 2]")]
    public class AVCodesAircraftCode
    {
        [ReferenceSingle("td[position() = 1]")]
        public string IATACode { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string ICAOCode { get; set; }

        [ReferenceSingle("td[position() = 3]")]
        public string ManufacturerAndAircraftTypeOrModel { get; set; }

        [ReferenceSingle("td[position() = 4]")]
        public string WakeCategory { get; set; }
    }
}
