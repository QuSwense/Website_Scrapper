using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class WikiCurrentHeadOfStatesAndGovModel : ScrapObject<object>
    {
        public ScrapString CountryName { get; set; }
        public List<WikiHOSCommonHead> HeadOfStates { get; set; }
        public List<WikiHOSCommonHead> HeadOfGovs { get; set; }
    }

    public class WikiHOSCommonHead
    {
        public string Type { get; set; }
        public string TypeUrl { get; set; }
        public string CurrentPerson { get; set; }
        public string CurrentPersonUrl { get; set; }
        // denote acting, transitional, temporary leaders, or representatives
        public bool IsTransitional { get; set; }
    }
}
