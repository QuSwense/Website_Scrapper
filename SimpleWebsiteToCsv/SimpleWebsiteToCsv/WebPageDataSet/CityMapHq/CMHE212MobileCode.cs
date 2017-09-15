using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using SimpleWebsiteToCsv.Metadata.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.CityMapHq
{
    [QSReferenceFromDataSource("//div[@class='padtop padbottom padright']/div/p[position() = 1]")]
    [QSURIDataSource(Online = "http://www.citymaphq.com/codes/e212.html")]
    public class CMHE212MobileCode : CMHCode<int>
    {
        public CMHCountryGeneralInformation General { get; set; }
    }

    [QSUriRTTIDataSource("SimpleWebsiteToCsv.WebPageDataSet.CityMapHq.E212MobileCode", "RegionURL")]
    [ReferenceSingle("//h3[contains(text(), 'General Information')]/following-sibling::div[@class='factlist col']/table")]
    public class CMHCountryGeneralInformation
    {
        [ReferenceSingle("//tr[position() = 1]/td[@class='value']")]
        public string Name { get; set; }

        [ReferenceSingle("//tr[position() = 2]/td[@class='value']")]
        public string Region { get; set; }

        [ReferenceSingle("//tr[position() = 3]/td[@class='value']")]
        public string SubRegion { get; set; }

        [ReferenceSingle("//tr[position() = 4]/td[@class='value']")]
        public string AreaKm2 { get; set; }
    }
}
