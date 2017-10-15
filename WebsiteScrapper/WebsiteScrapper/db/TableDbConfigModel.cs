using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteScrapper.db
{
    public class TableDbConfigModel
    {
        public string Name { get; set; }
        public Dictionary<string, ColumnDbConfigModel> Columns { get; set; }
        public TableMetadataModel Metadata { get; set; }

        public TableDbConfigModel()
        {
            Columns = new Dictionary<string, ColumnDbConfigModel>();
        }
    }
}
