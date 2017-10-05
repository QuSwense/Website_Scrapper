using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class ISO31661Alpha2Model : ScrapObject<object>
    {
        public ScrapTable ISO31661Alpha2OfficialTable { get; set; }
        public ScrapTable ISO31661Alpha2ExceptionalTable { get; set; }
    }
}
