using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class DBTableAttribute : Attribute
    {
        public string TableName { get; set; }
        public string Description { get; set; }
        public string ColumnNamePrefix { get; set; }
    }
}
