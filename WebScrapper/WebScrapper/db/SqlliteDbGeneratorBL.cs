using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Web;

namespace WebScrapper.Db
{
    /// <summary>
    /// https://www.techonthenet.com/sqlite/datatypes.php
    /// </summary>
    public class SqlliteDbGeneratorBL : DbGeneratorBL
    {
        private SQLiteConnection dbConnection;

        public SqlliteDbGeneratorBL(DbConfigModel config) : base(config) { }

        protected override void CreateDbFile()
        {
            SQLiteConnection.CreateFile(DbConfig.AppTopic + ".sqlite");
        }

        public override void OpenConnection()
        {
            connectionString = string.Format("Data Source={0}.sqlite;Version=3;", DbConfig.AppTopic);
            dbConnection = new SQLiteConnection(connectionString);
            dbConnection.Open();
        }

        public override void CloseConnection()
        {
            dbConnection.Close();
        }

        protected override void GenerateTables()
        {
            base.GenerateTables();
        }

        private string GetConstraints(ColumnDbConfigModel colConfig)
        {
            string constraints = "";
            if (colConfig.Unique) constraints = "UNIQUE";
            if (colConfig.IsPrimaryKey) constraints += " PRIMARY KEY";

            return constraints;
        }

        internal override string GetDataType(ColumnDbConfigModel colConfig)
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

        protected override void ExecuteDDL(string sql)
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
            ExecuteDDL(sqlDML);
        }

        private string CreateUpdateDDL(string name, Dictionary<string, ColumnScrapModel> rowData)
        {
            List<string> setDMLs = new List<string>();
            List<string> whereDMLs = new List<string>();

            foreach (KeyValuePair<string, ColumnScrapModel> kv in rowData)
            {
                setDMLs.Add(kv.Key + " = " + kv.Value);
                if (kv.Value.IsPk)
                {
                    whereDMLs.Add(kv.Key + " = " + kv.Value);
                }
            }

            return "UPDATE " + name + " SET " + string.Join(",", setDMLs) + " WHERE " + string.Join(" AND ", whereDMLs);
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
            List<string> whereDMLs = new List<string>();
            int count = 0;

            foreach (KeyValuePair<string, ColumnScrapModel> kv in rowData)
            {
                if (kv.Value.IsPk)
                {
                    whereDMLs.Add(kv.Key + " = " + kv.Value);
                }
            }
            
            using (SQLiteCommand fmd = dbConnection.CreateCommand())
            {
                fmd.CommandText = "SELECT COUNT(*) FROM " + name + " WHERE " + string.Join(" AND ", whereDMLs);
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

        internal override string GetDataType(Type propertyType)
        {
            if (propertyType == typeof(string))
                return "TEXT";
            else if (propertyType == typeof(int))
                return "INTEGER";
            else if (propertyType == typeof(double))
                return "REAL";
            else if (propertyType == typeof(DateTime))
                return "NUMERIC";
            else
                return "TEXT";
        }
    }
}
