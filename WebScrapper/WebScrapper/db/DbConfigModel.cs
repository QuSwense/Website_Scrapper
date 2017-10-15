using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.common;

namespace WebScrapper.db
{
    public class DbConfigModel
    {
        public string FolderName { get; set; }
        public Dictionary<string, TableDbConfigModel> TableDbConfigs { get; set; }
        public Dictionary<string, EnumConfigModel> EnumConfigs { get; set; }

        public DbConfigModel(string appfolder)
        {
            FolderName = appfolder;
            TableDbConfigs = new Dictionary<string, TableDbConfigModel>();
            EnumConfigs = new Dictionary<string, EnumConfigModel>();
        }

        public void Read()
        {
            ReadTableColumnsConfig();
            ReadTableMetadata();
            ReadEnumConfig();
        }

        private void ReadEnumConfig()
        {
            string tableEnumFilePath = Path.Combine(FolderName, "dbscripts", "table_enum.csv");

            using (var txtreader = new StreamReader(tableEnumFilePath))
            {
                string line = null;
                while ((line = txtreader.ReadLine()) != null)
                {
                    EnumConfigModel enumConfig;
                    string[] split = line.Split(new char[] { ',' });

                    WebScrapperException.IsValidId(split[0], "Enum Name is Mandatory");

                    if (EnumConfigs.ContainsKey(split[0]))
                        enumConfig = EnumConfigs[split[0]];
                    else
                    {
                        enumConfig = new EnumConfigModel();
                        enumConfig.Name = split[0];
                        EnumConfigs.Add(split[0], enumConfig);
                    }

                    int enumVal = Convert.ToInt32(split[1]);
                    WebScrapperException.Assert(() => !enumConfig.Values.ContainsKey(enumVal), "Enum Value is duplicated");

                    EnumValueConfigModel enumValConfig = new EnumValueConfigModel();
                    enumConfig.Values.Add(enumVal, enumValConfig);
                    enumValConfig.Description = split[2];
                }
            }
        }

        private void ReadTableMetadata()
        {
            string tableMetadataFilePath = Path.Combine(FolderName, "dbscripts", "table_metadata.csv");

            using (var txtreader = new StreamReader(tableMetadataFilePath))
            {
                string line = null;
                while ((line = txtreader.ReadLine()) != null)
                {
                    TableDbConfigModel tableDbConfig;
                    string[] split = line.Split(new char[] { ',' });

                    WebScrapperException.IsValidId(split[0], "Table Name is Mandatory");

                    if (TableDbConfigs.ContainsKey(split[0]))
                        tableDbConfig = TableDbConfigs[split[0]];
                    else
                    {
                        tableDbConfig = new TableDbConfigModel();
                        tableDbConfig.Name = split[0];
                        TableDbConfigs.Add(split[0], tableDbConfig);
                    }

                    tableDbConfig.Metadata = new TableMetadataModel();
                    tableDbConfig.Metadata.Display = split[1];
                    tableDbConfig.Metadata.Reference = split[2];
                }
            }
        }

        private void ReadTableColumnsConfig()
        {
            string tableColumnsFilePath = Path.Combine(FolderName, "dbscripts", "table_columns.csv");

            using (var txtreader = new StreamReader(tableColumnsFilePath))
            {
                string line = null;
                while((line = txtreader.ReadLine()) != null)
                {
                    TableDbConfigModel tableDbConfig;
                    string[] split = line.Split(new char[] { ',' });

                    WebScrapperException.IsValidId(split[0], "Table Name is Mandatory");

                    if (TableDbConfigs.ContainsKey(split[0]))
                        tableDbConfig = TableDbConfigs[split[0]];
                    else
                    {
                        tableDbConfig = new TableDbConfigModel();
                        tableDbConfig.Name = split[0];
                        TableDbConfigs.Add(split[0], tableDbConfig);
                    }

                    ColumnDbConfigModel columnDbConfig = new ColumnDbConfigModel();

                    WebScrapperException.IsValidId(split[1], "Column Name is Mandatory");
                    WebScrapperException.IsValidEnum(split[3], typeof(EDataTypeDbConfigModel),
                        "Data Type for a Column is Mandatory");

                    WebScrapperException.Assert(() => !tableDbConfig.Columns.ContainsKey(split[1]), "Column name already exists");
                    tableDbConfig.Columns.Add(split[1], columnDbConfig);

                    columnDbConfig.Name = split[1];
                    columnDbConfig.Display = split[2];
                    columnDbConfig.DataType = (EDataTypeDbConfigModel)Enum.Parse(typeof(EDataTypeDbConfigModel), split[3]);
                    columnDbConfig.Size = Convert.ToInt32(split[4]);
                    columnDbConfig.Precision = Convert.ToInt32(split[5]);
                    columnDbConfig.Unique = Convert.ToBoolean(split[6]);
                    columnDbConfig.PrimaryKey = Convert.ToBoolean(split[7]);
                }
            }
        }

        public TableDbConfigModel GetTable(string name)
        {
            TableDbConfigModel tableDb;

            WebScrapperException.Assert(() => TableDbConfigs.ContainsKey(name),
                    "Table not found");
            tableDb = TableDbConfigs[name];

            return tableDb;
        }
    }
}
