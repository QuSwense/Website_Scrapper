using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.DBTableDataSet
{
    [DBTable(TableName = "region",
        Description = "This table contains code representation of each country, regions as used by different international institutions for its purpose. It also contains standard ISO code representations.",
        ColumnNamePrefix = "rgn_")]
    public class RegionData
    {
        public string iso3166a2 { get; set; }
        public int m49 { get; set; }
        public string iso3166a3 { get; set; }
        public string name { get; set; }
        public int parent_m49 { get; set; }
        public int flag_region_type { get; set; }
        public int unsd_group { get; set; }
        public bool drives_on { get; set; }
        public DateTime iso3166_introduced { get; set; }
        public string cctld { get; set; }
        public bool sovereign { get; set; }
    }
}
