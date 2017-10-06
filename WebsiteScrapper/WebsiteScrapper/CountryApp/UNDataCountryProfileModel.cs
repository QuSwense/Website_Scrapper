using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class UNDataCountryProfileModel : ScrapObject<object>
    {
        public ScrapString Region { get; set; }
        public ScrapString SurfaceAreaSqKm { get; set; }
        public ScrapString CapitalCity { get; set; }
        public ScrapString Currency { get; set; }
        public ScrapString UNMembershipDate { get; set; }
    }
}
