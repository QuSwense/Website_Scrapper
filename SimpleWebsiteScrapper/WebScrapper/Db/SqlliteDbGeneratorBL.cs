using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Config;
using WebScrapper.Web;
using WebScrapper.Db.Config;
using System.Data.Common;

namespace WebScrapper.Db
{
    /// <summary>
    /// https://www.techonthenet.com/sqlite/datatypes.php
    /// </summary>
    public class SqlliteDbGeneratorBL : DbGeneratorBL
    {
        private SQLiteConnection dbConnection;

        public SqlliteDbGeneratorBL(DbConfig config) : base(config) { }

        protected override void CreateDbFile()
        {
            dbFile = ConfigHelper.GetDbConfigPath(DbConfig.AppTopic, ".sqlite");
            connectionString = string.Format("Data Source={0};Version=3;", dbFile);
            SQLiteConnection.CreateFile(dbFile);
        }

        public override void OpenConnection()
        {
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

        private string GetConstraints(ColumnDbConfig colConfig)
        {
            string constraints = "";
            if (colConfig.Unique) constraints = "UNIQUE";
            if (colConfig.IsPrimaryKey) constraints += " PRIMARY KEY";

            return constraints;
        }

        internal override string GetDataType(ColumnDbConfig colConfig)
        {
            switch(colConfig.DataType)
            {
                case EDataTypeDbConfig.BOOLEAN:
                    return "BOOLEAN";
                case EDataTypeDbConfig.DECIMAL:
                    return "DECIMAL(" + colConfig.Size + ", " + colConfig.Precision + ")";
                case EDataTypeDbConfig.ENUM:
                case EDataTypeDbConfig.NUMBER:
                    return "INTEGER";
                case EDataTypeDbConfig.STRING:
                    return "VARCHAR(" + colConfig.Size + ")";
                default:
                    return "TEXT";
            }
        }

        public override void ExecuteDDL(string sql)
        {
            if(!string.IsNullOrEmpty(sql))
            {
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }
        }

        public override DbDataReader ExecuteDML(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                using (SQLiteCommand fmd = dbConnection.CreateCommand())
                {
                    fmd.CommandText = sql;
                    fmd.CommandType = CommandType.Text;
                    return fmd.ExecuteReader();
                }
            }

            return null;
        }

        public override void SaveOrUpdate(string name, List<TableDataColumnModel> rowData)
        {
            QueryExecutor queryExecutor = new QueryExecutor(this);
            queryExecutor.SetTable(name);
            queryExecutor.SetRow(rowData);
            queryExecutor.SaveOrUpdate();
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

        public override void InsertTableMetadata(string name)
        {
            throw new NotImplementedException();
        }

        //internal override EDataTypeDbConfig GetDataType(Type propertyType)
        //{
        //    if (propertyType == typeof(string))
        //        return "TEXT";
        //    else if (propertyType == typeof(int))
        //        return "INTEGER";
        //    else if (propertyType == typeof(double))
        //        return "REAL";
        //    else if (propertyType == typeof(DateTime))
        //        return "NUMERIC";
        //    else
        //        return "TEXT";
        //}
    }
}
