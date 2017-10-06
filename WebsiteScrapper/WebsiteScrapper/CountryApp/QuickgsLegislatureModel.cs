using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class QuickgsLegislatureModel : ScrapObject<object>
    {
        public ScrapString CountryName { get; set; }
        public ScrapString ParliamentName { get; set; }
        public ScrapString ParliamentType { get; set; }
        public ScrapString LowerSeats { get; set; }
        public ScrapString UpperSeats { get; set; }
        public ScrapString Seats { get; set; }
    }
}
