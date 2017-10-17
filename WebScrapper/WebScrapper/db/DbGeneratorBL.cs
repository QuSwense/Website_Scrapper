using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WebScrapper.Web;
using WebScrapper.Db.Meta;

namespace WebScrapper.Db
{
    public abstract class DbGeneratorBL
    {
        public DbConfigModel DbConfig { get; protected set; }
        protected string connectionString;

        public DbGeneratorBL(DbConfigModel dbconfig)
        {
            DbConfig = dbconfig;
        }

        public virtual void Generate()
        {
            CreateDbFile();

            try
            {
                OpenConnection();
                GenerateTables();
            }
            finally
            {
                CloseConnection();
            }
        }

        protected virtual void GenerateTables()
        {
            // Create Metadata tables
            CreateTable<TableMetadataModel>();
            CreateTable<TableColumnsModel>();
            CreateTable<TableColumnsReferenceModel>();

            foreach (KeyValuePair<string, Dictionary<string, ColumnDbConfigModel>> kv in DbConfig.TableDbConfigs)
            {
                Dictionary<string, ColumnDbConfigModel> colDbModel = kv.Value;

                string tableName = kv.Key;
                List<string> pks = new List<string>();
                List<string> ddlQueryCols = new List<string>();

                foreach (KeyValuePair<string, ColumnDbConfigModel> colconfig in colDbModel)
                {
                    string colName = colconfig.Key;
                    string colProp = GetDataType(colconfig.Value);
                    
                    if (colconfig.Value.Unique) colProp += " UNQIUE";

                    if (colconfig.Value.IsPrimaryKey) pks.Add(colName);

                    ddlQueryCols.Add(colName + " " + colProp);
                }

                CreateTable(tableName, pks, ddlQueryCols);
            }
        }

        public virtual void CloseConnection()
        {
            throw new NotImplementedException();
        }

        public virtual void OpenConnection()
        {
            throw new NotImplementedException();
        }

        protected virtual void CreateDbFile()
        {
            throw new NotImplementedException();
        }

        public virtual void AddRow(string name, Dictionary<string, ColumnScrapModel> rowData)
        {
            throw new NotImplementedException();
        }

        protected void CreateTable<T>()
        {
            Type tableType = typeof(T);

            string tableName = "";
            List<string> pks = new List<string>();
            List<string> ddlQueryCols = new List<string>();

            DDTableAttribute tableattr = tableType.GetCustomAttribute<DDTableAttribute>();
            tableName = (tableattr != null)? tableattr.Name : tableType.Name;

            PropertyInfo[] classProperties = tableType.GetProperties(BindingFlags.Public);

            foreach (PropertyInfo prop in classProperties)
            {
                string colName = "";
                string colProp = "";

                DDColumnAttribute colAttr = prop.GetCustomAttribute<DDColumnAttribute>();
                colName = (colAttr != null) ? colAttr.Name : prop.Name;

                colProp += GetDataType(prop.PropertyType);

                DDNotNullAttribute notNullAttr = prop.GetCustomAttribute<DDNotNullAttribute>();
                if (notNullAttr != null) colProp += " NOT NULL";

                DDPrimaryKeyAttribute pkAttr = prop.GetCustomAttribute<DDPrimaryKeyAttribute>();
                if (pkAttr != null) pks.Add(colName);

                ddlQueryCols.Add(colName + " " + colProp);
            }

            CreateTable(tableName, pks, ddlQueryCols);
        }

        protected void CreateTable(string tableName, List<string> pks, List<string> ddlQueryCols)
        {
            string sqlDdl = "CREATE TABLE " + tableName + " ( " + string.Join(",", ddlQueryCols) +
                ", PRIMARY KEY (" + string.Join(",", pks) + "))";
            ExecuteDDL(sqlDdl);
        }

        internal abstract string GetDataType(Type propertyType);
        protected abstract void ExecuteDDL(string sql);
        internal abstract string GetDataType(ColumnDbConfigModel colConfig);
    }
}
