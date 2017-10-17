using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper.Db
{
    public class TableDbConfigModel
    {
        public string Name { get; set; }
        public Dictionary<string, ColumnDbConfigModel> Columns { get; set; }
        public TableMetadataConfigModel Metadata { get; set; }

        public TableDbConfigModel()
        {
            Columns = new Dictionary<string, ColumnDbConfigModel>();
        }

        public string GetColumnData(string columnName, string value)
        {
            ColumnDbConfigModel columnConfig = Columns[columnName];

            switch(columnConfig.DataType)
            {
                case EDataTypeDbConfigModel.BOOLEAN:
                    return value;
                case EDataTypeDbConfigModel.STRING:
                    return "'" + value + "'";
                default:
                    return value;
            }
        }
    }
}
