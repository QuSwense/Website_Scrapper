using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class GeoNamesDatabaseModel : ScrapObject<object>
    {
        public ScrapTable GeoNamesCountries { get; set; }
        public ScrapObject<List<GeoNamesCountryInfoModel>> CountriesInfo { get; set; }
    }
}
