using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class CIAWorldFactbookCountryPeopleAndSocietyModel : ScrapObject<object>
    {
        public ScrapString NationalityNoun { get; set; }
        public ScrapString NationalityAdjective { get; set; }
    }
}
