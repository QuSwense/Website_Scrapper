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
using System.Collections;
using DynamicDatabase.Types;

namespace DynamicDatabase
{
    /// <summary>
    /// The main database class which contains and stores all the necessary states, data, and
    /// objects which forms a the database basis.
    /// This template class uses other classes that is provided as a template parameters by
    /// the derived class.
    /// This Database context class is responsible for all sorts of database activities.
    /// </summary>
    public class DynamicDbContext : IDbContext
    {
        #region Properties

        /// <summary>
        /// The Database data type context
        /// </summary>
        public IDataTypeContext DataType { get; protected set; }

        /// <summary>
        /// The connection object
        /// </summary>
        public IDynamicDbConnection Connection { get; protected set; }

        /// <summary>
        /// The list of all tables in the database
        /// </summary>
        public Dictionary<string, IDbTable> Tables { get; protected set; }

        /// <summary>
        /// The object which builds and execute any database query of rthe database context.
        /// To retrieve the current sql query executed in the database use the <see cref="SQL"/>
        /// property
        /// </summary>
        public IDbCommand DbCommand { get; protected set; }

        #endregion Properties

        #region Initialize

        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamicDbContext()
        {
            Connection = DynamicDbFactory.Create<IDynamicDbConnection>();
            DbCommand = DynamicDbFactory.Create<IDbCommand>();
        }

        /// <summary>
        /// Initialize with db database file and name
        /// </summary>
        /// <param name="arg">The argument to initialize conext object</param>
        public virtual void Initialize(ArgsContextInitialize arg)
        {
            if (arg == null) throw new Exception("The context initialize argument is null");
            Connection.Initialize(arg.DbFilePath, arg.Name, arg.ConnectionString);
            DbCommand.Initialize(this, Connection);
        }

        #endregion Initialize

        #region Database

        /// <summary>
        /// An empty virtual method whose purpose is to create database
        /// </summary>
        public virtual void CreateDatabase()
        {
            Connection.CreateDatabase();
        }
        
        /// <summary>
        /// An empty virtual method whose purpose is to check if the database already exists or not
        /// </summary>
        /// <returns></returns>
        public virtual bool DatabaseExists()
        {
            return Connection.DatabaseExists();
        }

        /// <summary>
        /// An empty virtual method whose purpose is to delete database
        /// </summary>
        public virtual void DeleteDatabase()
        {
            Connection.DeleteDatabase();
        }

        #endregion Database

        #region Connection

        /// <summary>
        /// Use this method to open connection to the database
        /// </summary>
        public virtual void Open()
        {
            Connection.Open();
        }

        /// <summary>
        /// Use this method to close connection to the database
        /// </summary>
        public virtual void Close()
        {
            Connection.Close();
        }

        #endregion Connection

        #region Create

        /// <summary>
        /// Create a new table in the database using a class type. That class should be decorated with 
        /// Database attribute <see cref="DDTableAttribute"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">The name of the table. Use this to overwrite the table attribute</param>
        public virtual void CreateTable<T>(string tableName = null)
        {
            Type tableType = typeof(T);
            DDTableAttribute tableattr = tableType.GetCustomAttribute<DDTableAttribute>();

            if (string.IsNullOrEmpty(tableName))
            {
                tableName = (tableattr != null) ? tableattr.Name : tableType.Name;
            }

            CreateTable(tableName,
                (dynTable) => dynTable.CreateTable(
                    tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance)));
        }

        /// <summary>
        /// Create the database from the config data dynamically
        /// </summary>
        public virtual void CreateTable(
            Dictionary<string, Dictionary<string, ConfigDbColumn>> tableColumnConfigs)
        {
            // Create App specific tables
            foreach (var kv in tableColumnConfigs)
            {
                CreateTable(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// Create a new table in the database from column config. If the table exists then throw error.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="configCols">The table column configurations to create the table</param>
        public virtual void CreateTable(string tableName,
            Dictionary<string, ConfigDbColumn> configCols)
        {
            CreateTable(tableName, (dynTable) => dynTable.CreateTable(configCols));
        }

        /// <summary>
        /// Create table(s) using the collection value
        /// </summary>
        /// <param name="value">The value with column data</param>
        public virtual void CreateTable(ICollection colData) { }

        /// <summary>
        /// Create table(s) using the collection value with the table name
        /// </summary>
        /// <param name="name">The table name</param>
        /// <param name="colData">The value with column data</param>
        public virtual void CreateTable(string name, ICollection colData) { }

        /// <summary>
        /// Delete a table
        /// </summary>
        /// <param name="tablename">The name of the table</param>
        public virtual void DeleteTable(string tablename)
        {
            IDbTable table = GetTable(tablename);
            table.Delete();
            Tables.Remove(tablename);
        }

        /// <summary>
        /// A generic private method to create table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fnTableCreate"></param>
        protected void CreateTable(string name, Action<IDbTable> fnTableCreate)
        {
            // Check if the tables data set exists if not create new
            if (Tables == null) Tables = new Dictionary<string, IDbTable>();

            if (Tables.ContainsKey(name)) throw new Exception(string.Format("The table {0} already exists", name));

            // Create the new table
            IDbTable dynTable = DynamicDbFactory.Create<IDbTable>();
            dynTable.Initialize(this, name);

            // The action method to create table
            fnTableCreate(dynTable);

            Tables.Add(name, dynTable);

            // Call Database command to create table
            DbCommand.CreateTable(dynTable);
        }

        #endregion Create

        #region Load

        /// <summary>
        /// Use this method to load data
        /// Load the table data and metdata from the database
        /// </summary>
        /// <param name="name"></param>
        public virtual void Load(string name)
        {
            LoadMetadata(name);
            LoadData(name);
        }

        /// <summary>
        /// Load the data type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        public virtual void Load<T>(string name)
        {
            Type tableType = typeof(T);
            DDTableAttribute tableattr = tableType.GetCustomAttribute<DDTableAttribute>();

            if (string.IsNullOrEmpty(name))
                name = (tableattr != null) ? tableattr.Name : tableType.Name;
            LoadMetadata(name);
            LoadData(name);
        }

        /// <summary>
        /// Use this method to load data
        /// Load the table data and metdata from the database
        /// </summary>
        /// <param name="name"></param>
        public void Load(string name, params string[] args)
        {
            LoadMetadata(name);
            Load(name, (dynTable) => {
                dynTable.LoadData(DbCommand.LoadData(name), args);
            });
        }

        /// <summary>
        /// Load the metadata of the table.
        /// </summary>
        /// <param name="name">The name of the table</param>
        public void LoadMetadata(string tablename)
        {
            IDbTable table = GetTable(tablename);
            Load(tablename, (dynTable) => {
                dynTable.LoadTableMetadata(DbCommand.LoadTableMetadata(tablename));
            });
        }

        /// <summary>
        /// Load the table data. This method is not public. We should always use <see cref="Load(string)"/>
        /// to load data into a table as Loading of metdata alongwith data is important
        /// </summary>
        /// <param name="name"></param>
        protected void LoadData(string tablename)
        {
            IDbTable table = GetTable(tablename);
            Load(tablename, (dynTable) => {
                dynTable.LoadData(DbCommand.LoadData(tablename));
            });
        }

        /// <summary>
        /// Load data / metadata from the database and store in memory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fnTableLoad"></param>
        private void Load(string tablename, Action<IDbTable> fnTableLoad)
        {
            IDbTable dynTable = GetTable(tablename);
            
            fnTableLoad(dynTable);

            Tables.Add(tablename, dynTable);
        }

        /// <summary>
        /// Clear the table data that is loaded in memory
        /// </summary>
        /// <param name="name"></param>
        public void Clear(string tablename)
        {
            IDbTable table = GetTable(tablename);
            if (table != null) table.Clear();
        }

        #endregion Load

        #region Insert

        /// <summary>
        /// Add or update a row using the unique keys.
        /// For this method to work it is mandatory that the tbale class is registered before with 
        /// <see cref="DynamicSortTable"/>
        /// </summary>
        /// <exception cref="Exception">If the table do not exists</exception>
        /// <param name="tablename"></param>
        /// <param name="ukeys"></param>
        /// <param name="row"></param>
        public virtual void AddOrUpdate(string tablename, IEnumerable<DbDataType> ukeys,
            IEnumerable<DbDataType> row)
        {
            IDbTable table = GetTable(tablename);

            table.AddorUpdate(ukeys, row);
        }

        /// <summary>
        /// Add or update a row using the the unique keys with column names.
        /// </summary>
        /// <exception cref="Exception">If the table do not exists</exception>
        /// <param name="tablename"></param>
        /// <param name="ukeys">The unique keys which is used to insert the data into table.</param>
        /// <param name="row">The row data to insert into table indexed by zero.</param>
        public virtual void AddOrUpdate(string tablename, IDictionary<string, DbDataType> ukeys,
            IEnumerable<DbDataType> row)
        {
            IDbTable table = GetTable(tablename);
            table.AddorUpdate(ukeys, row);
        }

        /// <summary>
        /// Add or update a row using the the unique keys with column names.
        /// </summary>
        /// <exception cref="Exception">If the table do not exists</exception>
        /// <param name="tablename"></param>
        /// <param name="ukeys">The unique keys which is used to insert the data into table.</param>
        /// <param name="row">The row data to insert into table indexed by column name.</param>
        public virtual void AddOrUpdate(string tablename, IDictionary<string, DbDataType> ukeys,
            IDictionary<string, DbDataType> row)
        { }

        /// <summary>
        /// Bulk add the table metadata information
        /// </summary>
        /// <param name="tableMetas"></param>
        public virtual void AddOrUpdate(Dictionary<string, ConfigDbTable> tableMetas)
        {
            Type tableMetaType = typeof(DbMetaTableModel);
            DDTableAttribute tableAttr = tableMetaType.GetCustomAttribute<DDTableAttribute>();

            if (tableAttr == null) throw new Exception("Table name attribute on DbMetaTableModel class not found");
            if (!Tables.ContainsKey(tableAttr.Name)) LoadMetadata(tableAttr.Name);

            Tables[tableAttr.Name].AddorUpdate(tableMetas);
        }

        /// <summary>
        /// Add or update
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        public virtual void AddOrUpdate(string name, List<TableDataColumnModel> row)
        {
            if (!Tables.ContainsKey(name)) LoadMetadata(name);

            Tables[name].AddorUpdate(row);
        }

        /// <summary>
        /// Add or update
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        public virtual void AddOrUpdate<T>(string name, List<TableDataColumnModel> row)
        {
            Type tableMetaType = typeof(DbMetaTableModel);
            DDTableAttribute tableAttr = tableMetaType.GetCustomAttribute<DDTableAttribute>();

            name = (tableAttr == null) ? name : tableAttr.Name;

            AddOrUpdate(name, row);
        }

        #endregion Insert

        #region Helper

        /// <summary>
        /// A helper internal method to get the table if present.
        /// If not then it throws exception.
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        protected IDbTable GetTable(string tablename)
        {
            if (string.IsNullOrEmpty(tablename)) throw new Exception("The name of the table is delete");
            if (!Tables.ContainsKey(tablename)) throw new Exception(tablename + ": No such table found");
            return Tables[tablename];
        }

        #endregion Helper

        ///// <summary>
        ///// This is a virtual method which needs to be overriden in the derived class.
        ///// Different Databse will have different data types
        ///// </summary>
        ///// <param name="propertyType"></param>
        ///// <returns></returns>
        //public virtual string GetDataType(Type propertyType)
        //{
        //    return "TEXT";
        //}

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
                    if (Connection != null) Connection.Dispose();
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
