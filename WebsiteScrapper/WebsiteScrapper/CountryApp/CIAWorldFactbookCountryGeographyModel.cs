using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class CIAWorldFactbookCountryGeographyModel : ScrapObject<object>
    {
        public ScrapString Location { get; set; }
        public ScrapString Coordinates { get; set; }
        public ScrapString TotalArea { get; set; }
        public ScrapString LandArea { get; set; }
        public ScrapString WaterArea { get; set; }
        public ScrapString CountryComparisionByAreaRank { get; set; }
        public ScrapString TotalLandBoundaries { get; set; }
        public ScrapString BorderCountries { get; set; }
        public ScrapString Coastline { get; set; }
        public ScrapString MeanElevation { get; set; }
        public ScrapString LowestPoint { get; set; }
        public ScrapString HighestPoint { get; set; }
    }
}
