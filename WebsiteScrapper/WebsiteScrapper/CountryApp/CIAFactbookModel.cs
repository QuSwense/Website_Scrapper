using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class CIAFactbookModel : ScrapObject<object>
    {
        public List<CIAWorldFactbookCountryGeographyModel> CountriesGeography { get; set; }
        public List<CIAWorldFactbookCountryPeopleAndSocietyModel> CountriesPeopleAndSociety { get; set; }
        public List<CIAWorldFactbookCountryGovernmentModel> CountriesGovernment { get; set; }
    }
}
