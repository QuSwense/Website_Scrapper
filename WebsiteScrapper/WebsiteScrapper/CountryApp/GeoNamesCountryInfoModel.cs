using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class GeoNamesCountryInfoModel : ScrapObject<object>
    {
        public ScrapString CountryNameOtherLangUrl { get; set; }
        public ScrapString Currency { get; set; }
        public ScrapList Neighbours { get; set; }
        public ScrapList Languages { get; set; }
        public ScrapString PostalCodeFormat { get; set; }
        public ScrapString ISOAdministrativeDivisionsUrl { get; set; }
        public ScrapString LargestCitiesUrl { get; set; }
        public ScrapString HighestMountainsUrl { get; set; }
        public ScrapTable OtherLanguageCountriesNames { get; set; }
        public ScrapTable ISOAdminDivs { get; set; }
        public ScrapTable LargestCities { get; set; }
        public ScrapTable HighestMountains { get; set; }
    }
}
