using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class DBColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }
        public ESqlliteType SqlliteType { get; set; }
        public string MySqlType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// M is the total number of digits
        /// </summary>
        public int M { get; set; }
        /// <summary>
        /// D is the number of digits following the decimal point
        /// </summary>
        public int D { get; set; }

        public DBColumnAttribute()
        {
            IsPrimaryKey = false;
        }
    }
}
