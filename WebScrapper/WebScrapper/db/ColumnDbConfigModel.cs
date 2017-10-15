using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper.db
{
    public class ColumnDbConfigModel
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public EDataTypeDbConfigModel DataType { get; set; }
        public int Size { get; set; }
        public int Precision { get; set; }
        public bool Unique { get; set; }
        public bool PrimaryKey { get; set; }
    }
}
