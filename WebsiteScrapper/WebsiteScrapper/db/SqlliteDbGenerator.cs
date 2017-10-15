using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteScrapper.db
{
    public class SqlliteDbGenerator
    {
        public DbConfig DbConfig;
        private SQLiteConnection dbConnection;

        public SqlliteDbGenerator(DbConfig config)
        {
            DbConfig = config;
        }

        public void Generate()
        {
            SQLiteConnection.CreateFile(DbConfig.FolderName + ".sqlite");
            dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            dbConnection.Open();
            GenerateTables();
            dbConnection.Close();
        }

        public void GenerateTables()
        {
            foreach(KeyValuePair<string, TableDbConfigModel> kv in DbConfig.TableDbConfigs)
            {
                TableDbConfigModel tableDb = kv.Value;

                string tableCreate = "CREATE TABLE " + kv.Key + "(";

                foreach(KeyValuePair<string, ColumnDbConfigModel> kvcol in tableDb.Columns)
                {
                    ColumnDbConfigModel colConfig = kvcol.Value;
                    tableCreate += colConfig.Name + " " + GetDataType(colConfig) + " " + GetConstraints(colConfig) + ",";
                }

                tableCreate = tableCreate.Remove(tableCreate.Length - 1);
                tableCreate += ")";
                ExecuteDML(tableCreate);
            }
        }

        private string GetConstraints(ColumnDbConfigModel colConfig)
        {
            string constraints = "";
            if (colConfig.Unique) constraints = "UNIQUE";
            if (colConfig.PrimaryKey) constraints += " PRIMARY KEY";

            return constraints;
        }

        private string GetDataType(ColumnDbConfigModel colConfig)
        {
            switch(colConfig.DataType)
            {
                case EDataTypeDbConfigModel.BOOLEAN:
                    return "BOOLEAN";
                case EDataTypeDbConfigModel.DECIMAL:
                    return "DECIMAL(" + colConfig.Size + ", " + colConfig.Precision + ")";
                case EDataTypeDbConfigModel.ENUM:
                case EDataTypeDbConfigModel.NUMBER:
                    return "INTEGER";
                case EDataTypeDbConfigModel.STRING:
                    return "VARCHAR(" + colConfig.Size + ")";
                default:
                    return "TEXT";
            }
        }

        private void ExecuteDML(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
        }
    }
}
