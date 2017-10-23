using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Config;
using DynamicDatabase.Meta;
using WebCommon.Extn;
using System.IO;
using DynamicDatabase.Model;

namespace DynamicDatabase
{
    /// <summary>
    /// The main database class which contains and stores all the necessary states, data, and
    /// objects which forms a the database basis.
    /// This template class uses other classes that is provided as a template parameters by
    /// the derived class.
    /// This Database context class is responsible for all sorts of database activities.
    /// </summary>
    /// <typeparam name="TDynTable">Represents a table of the database</typeparam>
    /// <typeparam name="TDbCommand">Used to issue command to database</typeparam>
    /// <typeparam name="TDbConnection">Database connection class</typeparam>
    /// <typeparam name="TDynRow">Represents a row of the table of the database</typeparam>
    /// <typeparam name="TDynColMetadata">The metadata of a table which represents a single column information</typeparam>
    public class DynamicDbContext<
        TDynTable,
        TDbCommand,
        TDbConnection,
        TDynRow, 
        TDynColMetadata
        > : IDisposable, IDbContext
        where TDynTable : DynamicTable<
            TDynRow,
            TDynColMetadata
            >, new()
        where TDbCommand : DynamicDbCommand<
            TDynTable,
            TDynRow,
            TDynColMetadata,
            TDbConnection>, new()
        where TDbConnection : DbConnection, new()
        where TDynRow : DynamicRow, new()
        where TDynColMetadata : DynamicColumnMetadata, new()
    {
        /// <summary>
        /// The name of the database
        /// </summary>
        public string DbName { get; protected set; }

        /// <summary>
        /// The full local path of the database file
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The extension of the database file
        /// </summary>
        public virtual string FileExtension { get; protected set; }

        /// <summary>
        /// Get the full path of the database file including the name
        /// </summary>
        public virtual string FullDbFileName
        {
            get { return Path.Combine(FullPath, DbName); }
        }

        /// <summary>
        /// The list of all tables in the database
        /// </summary>
        public Dictionary<string, TDynTable> Tables { get; protected set; }

        /// <summary>
        /// The object which builds and execute any database query of rthe database context.
        /// To retrieve the current sql query executed in the database use the <see cref="SQL"/>
        /// property
        /// </summary>
        public TDbCommand DbCommand { get; protected set; }

        /// <summary>
        /// The database connection object
        /// </summary>
        public TDbConnection ConnectionCtx { get; protected set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamicDbContext() { }

        /// <summary>
        /// Constructor with db database file and name
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        public DynamicDbContext(string dbfilepath, string name)
        {
            DbName = name;
            FullPath = dbfilepath;
        }

        /// <summary>
        /// Constructor with db file path and name and connection string
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public DynamicDbContext(string dbfilepath, string name, string connectionString)
        {
            DbName = name;
            FullPath = dbfilepath;
        }

        /// <summary>
        /// Construct the database context using the connection string
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="connectionCtx"></param>
        public DynamicDbContext(string connection)
        {
            ConnectionCtx = (TDbConnection)Activator.CreateInstance(typeof(TDbConnection), connection);
        }

        /// <summary>
        /// An empty virtual method whose purpose is to create database
        /// </summary>
        public virtual void CreateDatabase() { }

        /// <summary>
        /// Create the database from the config file dynamically
        /// </summary>
        public virtual void CreateDatabase(
            Dictionary<string, Dictionary<string, ConfigDbColumn>> TableColumnConfigs)
        {
            // Create App specific tables
            foreach (var kv in TableColumnConfigs)
            {
                CreateTable(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// An empty virtual method whose purpose is to check if the database already exists or not
        /// </summary>
        /// <returns></returns>
        public virtual bool DatabaseExists() { return false; }

        /// <summary>
        /// An empty virtual method whose purpose is to delete database
        /// </summary>
        public virtual void DeleteDatabase() { }

        /// <summary>
        /// Use this method to open connection to the database
        /// </summary>
        public virtual void Open()
        {
            ConnectionCtx.Open();
        }

        /// <summary>
        /// Use this method to close connection to the database
        /// </summary>
        public virtual void Close()
        {
            ConnectionCtx.Close();
        }

        /// <summary>
        /// Create a new table in the database. If the table exists then throw error.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="configCols">The table column configurations to create the table</param>
        public virtual void CreateTable(Dictionary<string, Dictionary<string, ConfigDbColumn>> configCols)
        {
            foreach (var item in configCols)
            {
                CreateTable(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Create a new table in the database. If the table exists then throw error.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="configCols">The table column configurations to create the table</param>
        protected virtual void CreateTable(string tableName, 
            Dictionary<string, ConfigDbColumn> configCols)
        {
            CreateTable(tableName, (dynTable) => dynTable.CreateTable(configCols));
        }

        /// <summary>
        /// Create a new table in the database using a class type. That class should be decorated with 
        /// Database attributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CreateTable<T>()
        {
            Type tableType = typeof(T);

            DDTableAttribute tableattr = tableType.GetCustomAttribute<DDTableAttribute>();
            CreateTable((tableattr != null) ? tableattr.Name : tableType.Name,
                (dynTable) => dynTable.CreateTable(
                    tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance)));
        }

        /// <summary>
        /// A generic private method to create table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fnTableCreate"></param>
        private void CreateTable(string name, Action<TDynTable> fnTableCreate)
        {
            // Check if the tables data set exists if not create new
            if (Tables == null) Tables = new Dictionary<string, TDynTable>();

            if (Tables.ContainsKey(name)) throw new Exception(string.Format("The table {0} already exists", name));

            // Create the new table
            TDynTable dynTable = (TDynTable)Activator.CreateInstance(typeof(TDynTable), name);

            // The action method to create table
            fnTableCreate(dynTable);

            Tables.Add(name, dynTable);

            // Call Database command to create table
            DbCommand.CreateTable(dynTable);
        }

        /// <summary>
        /// Use this method to load data
        /// Load the table data and metdata from the database
        /// </summary>
        /// <param name="name"></param>
        public void Load(string name)
        {
            LoadMetadata(name);
            LoadData(name);
        }

        /// <summary>
        /// Load the metadata of the table.
        /// </summary>
        /// <param name="name">The name of the table</param>
        public void LoadMetadata(string name)
        {
            Load(name, (dynTable) => {
                dynTable.LoadTableMetadata(DbCommand.LoadTableMetadata(name));
            });
        }

        /// <summary>
        /// Load the table data. This method is not public. We should always use <see cref="Load(string)"/>
        /// to load data into a table as Loading of metdata alongwith data is important
        /// </summary>
        /// <param name="name"></param>
        protected void LoadData(string name)
        {
            Load(name, (dynTable) => {
                dynTable.LoadData(DbCommand.LoadData(name));
            });
        }

        /// <summary>
        /// Load data / metadata from the database and store in memory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fnTableLoad"></param>
        private void Load(string name, Action<TDynTable> fnTableLoad)
        {
            if (Tables == null) Tables = new Dictionary<string, TDynTable>();
            TDynTable dynTable = Tables.ContainsKey(name) ? Tables[name] :
                (TDynTable)Activator.CreateInstance(typeof(TDynTable), name);

            fnTableLoad(dynTable);

            if (!Tables.ContainsKey(name)) Tables.Add(name, dynTable);
        }

        /// <summary>
        /// Bulk add the table metadata information
        /// </summary>
        /// <param name="tableMetas"></param>
        private void AddOrUpdateRows(Dictionary<string, ConfigDbTable> tableMetas)
        {
            Type tableMetaType = typeof(DbMetaTableModel);
            DDTableAttribute tableAttr = tableMetaType.GetCustomAttribute<DDTableAttribute>();

            if (tableAttr == null) throw new Exception("Table name attribute on DbMetaTableModel class not found");
            if (!Tables.ContainsKey(tableAttr.Name)) LoadMetadata(tableAttr.Name);

            Tables[tableAttr.Name].AddorUpdate(tableMetas);
        }

        /// <summary>
        /// This is a virtual method which needs to be overriden in the derived class.
        /// Different Databse will have different data types
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public virtual string GetDataType(Type propertyType)
        {
            return "TEXT";
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(Tables != null)
                    {
                        foreach(var item in Tables)
                        {
                            item.Value.Dispose();
                        }
                    }

                    if (DbCommand != null) DbCommand.Dispose();
                    if (ConnectionCtx != null) ConnectionCtx.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// This code added to correctly implement the disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
