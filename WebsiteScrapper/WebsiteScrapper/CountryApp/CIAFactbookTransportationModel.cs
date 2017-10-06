using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class CIAFactbookTransportationModel : ScrapObject<string>
    {
        public ScrapString NoOfRegisteredAirCarriers { get; set; }
        public ScrapString InventoryOfRegisteredAircraftOperatedByAirCarriers { get; set; }
        public ScrapString CivilAircraftRegCountryCodePrefix { get; set; }
        public ScrapString Airports { get; set; }
        public ScrapString AirportsWithPavedRunWaysTotal { get; set; }
        public List<ScrapString> AirportsWithPavedRunWays { get; set; }
        public ScrapString AirportsWithUnPavedRunWaysTotal { get; set; }
        public List<ScrapString> AirportsWithUnPavedRunWays{ get; set; }
        public ScrapString Heliports { get; set; }
    }
}
