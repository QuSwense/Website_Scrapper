using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.scrap;

namespace WebScrapper.db
{
    public class SqlliteDbGeneratorBL : DbGeneratorBL
    {
        private SQLiteConnection dbConnection;

        public SqlliteDbGeneratorBL(DbConfigModel config) : base(config) { }

        protected override void CreateDbFile()
        {
            SQLiteConnection.CreateFile(DbConfig.FolderName + ".sqlite");
        }

        public override void OpenConnection()
        {
            dbConnection = new SQLiteConnection(string.Format(
                "Data Source={0}.sqlite;Version=3;", DbConfig.FolderName));
            dbConnection.Open();
        }

        public override void CloseConnection()
        {
            dbConnection.Close();
        }

        protected override void GenerateTables()
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

        public override void AddRow(string name, Dictionary<string, ColumnScrapModel> rowData)
        {
            // Check if the primary key data exists
            int isexists = CheckRow(name, rowData);
            string sqlDML;

            if (isexists == 1)
                sqlDML = CreateUpdateDDL(name, rowData);
            else
                sqlDML = CreateInsertDDL(name, rowData);
            ExecuteDML(sqlDML);
        }

        private string CreateUpdateDDL(string name, Dictionary<string, ColumnScrapModel> rowData)
        {
            string sqlDML = "UPDATE " + name + " SET ";
            string wheres = " WHERE ";

            foreach (KeyValuePair<string, ColumnScrapModel> kv in rowData)
            {
                sqlDML += kv.Key + " = " + kv.Value + ",";
                if (kv.Value.IsPk)
                    wheres += kv.Key + " = " + kv.Value + " AND ";
            }

            sqlDML.Remove(sqlDML.Length - 1);
            wheres.Remove(wheres.Length - " AND ".Length);

            sqlDML += wheres;

            return sqlDML;
        }

        private string CreateInsertDDL(string name, Dictionary<string, ColumnScrapModel> rowData)
        {
            string sqlDML = "INSERT INTO " + name + "(";
            string values = " VALUES(";

            foreach (KeyValuePair<string, ColumnScrapModel> kv in rowData)
            {
                sqlDML += kv.Key + ",";
                values += kv.Value.Value + ",";
            }

            sqlDML = sqlDML.Remove(sqlDML.Length - 1);
            values = values.Remove(values.Length - 1);

            sqlDML += ") " + values + ")";

            return sqlDML;
        }

        private int CheckRow(string name, Dictionary<string, ColumnScrapModel> rowData)
        {
            string sqlDDL = "SELECT COUNT(*) FROM " + name + " WHERE ";
            int count = 0;

            foreach (KeyValuePair<string, ColumnScrapModel> kv in rowData)
            {
                if (kv.Value.IsPk)
                    sqlDDL += kv.Key + " = " + kv.Value + " AND ";
            }

            sqlDDL.Remove(sqlDDL.Length - " AND ".Length);

            using (SQLiteCommand fmd = dbConnection.CreateCommand())
            {
                fmd.CommandText = sqlDDL;
                fmd.CommandType = CommandType.Text;
                SQLiteDataReader r = fmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    count = r.GetInt32(0);
                }
            }

            return count;
        }
    }
}
