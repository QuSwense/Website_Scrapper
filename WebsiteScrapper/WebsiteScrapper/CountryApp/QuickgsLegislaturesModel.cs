using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class QuickgsLegislaturesModel : ScrapObject<object>
    {
        public ScrapTable AsiaOceaniaLegislatures { get; set; }
        public ScrapTable EuropeLegislatures { get; set; }
        public ScrapTable AmericaLegislatures { get; set; }
        public ScrapTable AfricaLegislatures { get; set; }
    }
}
