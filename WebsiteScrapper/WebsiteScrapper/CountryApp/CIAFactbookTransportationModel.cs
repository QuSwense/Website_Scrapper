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
    }
}
