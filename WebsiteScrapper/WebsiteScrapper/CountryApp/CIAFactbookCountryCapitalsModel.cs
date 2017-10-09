using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class CIAFactbookCountryCapitalsModel : ScrapObject<object>
    {
        public string Name { get; set; }
    }
}
