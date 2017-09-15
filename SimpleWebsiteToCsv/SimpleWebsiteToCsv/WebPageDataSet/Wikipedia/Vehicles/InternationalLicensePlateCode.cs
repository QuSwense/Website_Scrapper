using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Wiki
{
    [QSReference("Distinguishing signs of vehicles in international traffic (oval bumper sticker codes). The allocation of codes is maintained by the United Nations Economic Commission for Europe as the Distinguishing Signs Used on Vehicles in International Traffic (sometimes abbreviated to DSIT)")]
    [QSURIDataSource(Online = "http://www.wikiwand.com/en/List_of_international_vehicle_registration_codes")]
    [ReferenceCollection("//table[@class='sortable wikitable jquery-tablesorter']//tr[position() > 1]")]
    public class InternationalLicensePlateCode
    {
        [ReferenceSingle("td[position() = 1]")]
        public string Code { get; set; }

        [ReferenceSingle("td[position() = 1]")]
        [RegexReplace(Pattern = @"(\w+)\*")]
        public string isUnoffcial { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string Country { get; set; }

        [ReferenceSingle("td[position() = 3]")]
        public string StartDate { get; set; }

        [ReferenceSingle("td[position() = 5]")]
        public string Note { get; set; }
    }
}
