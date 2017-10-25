using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WebScrapper.Web;
using WebScrapper.Db.Meta;
using WebScrapper.Common;
using WebScrapper.Db.Config;
using WebScrapper.Db.Model;
using System.Data.Common;

namespace WebScrapper.Db
{
    public abstract class DbGeneratorBL
    {
        public DbConfig DbConfig { get; protected set; }
        protected string connectionString;
        protected string dbFile;

        public DbGeneratorBL(DbConfig dbconfig)
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
            CreateTable<DbMetaTableModel>();
            CreateTable<DbMetaTableColumnsModel>();
            CreateTable<DbMetaTableRowModel>();

            QueryGenerator queryGenerator;

            // Create App specific tables
            foreach (KeyValuePair<string, Dictionary<string, ColumnDbConfig>> kv in DbConfig.TableDbConfigs)
            {
                queryGenerator = new QueryGenerator();
                queryGenerator.CreateTable(kv.Key);
                
                foreach (KeyValuePair<string, ColumnDbConfig> colconfig in kv.Value)
                {
                    queryGenerator.Column(colconfig.Key, GetDataType(colconfig.Value),
                        colconfig.Value.Unique, false, colconfig.Value.IsPrimaryKey);
                }

                ExecuteDDL(queryGenerator.ToString());
            }

            // Add table metadata
            queryGenerator = new QueryGenerator();
            queryGenerator.Insert("mtbl");

            foreach (KeyValuePair<string, TableMetadataConfigModel> item in DbConfig.TableMetadata)
            {
                queryGenerator.Set("tnm");
                queryGenerator.Filter(typeof(string), item.Key);

                queryGenerator.Set("desc");
                queryGenerator.Filter(typeof(string), item.Value.Display);
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

        public virtual void SaveOrUpdate(string name, List<TableDataColumnModel> rowData)
        {
            throw new NotImplementedException();
        }

        protected void CreateTable<T>()
        {
            Type tableType = typeof(T);
            QueryGenerator queryGenerator = new QueryGenerator();

            DDTableAttribute tableattr = tableType.GetCustomAttribute<DDTableAttribute>();
            queryGenerator.CreateTable((tableattr != null)? tableattr.Name : tableType.Name);

            PropertyInfo[] classProperties = tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in classProperties)
            {
                DDColumnAttribute colAttr = prop.GetCustomAttribute<DDColumnAttribute>();
                DDNotNullAttribute notNullAttr = prop.GetCustomAttribute<DDNotNullAttribute>();
                DDPrimaryKeyAttribute pkAttr = prop.GetCustomAttribute<DDPrimaryKeyAttribute>();
                DDUniqueAttribute uniqueAttr = prop.GetCustomAttribute<DDUniqueAttribute>();

                queryGenerator.Column((colAttr != null) ? colAttr.Name : prop.Name,
                    GetDataType(prop.PropertyType), uniqueAttr != null,
                    notNullAttr != null, pkAttr != null);
            }

            ExecuteDDL(queryGenerator.ToString());
        }

        public EDataTypeDbConfig GetDataType(string table, string col)
        {
            return DbConfig.TableDbConfigs[table][col].DataType;
        }

        public abstract void InsertTableMetadata(string name);

        internal abstract string GetDataType(Type propertyType);
        public abstract DbDataReader ExecuteDML(string sql);
        public abstract void ExecuteDDL(string sql);
        internal abstract string GetDataType(ColumnDbConfig colConfig);
    }
}
