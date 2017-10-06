using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class ISO3166CountryDetailsModel : ScrapObject<object>
    {
        public ScrapString FullName { get; set; }
        public ScrapString Independent { get; set; }
        public ScrapString TerritoryName { get; set; }
        public ScrapTable AdditionalInfos { get; set; }
        public ScrapTable Subdivisions { get; set; }
    }
}
