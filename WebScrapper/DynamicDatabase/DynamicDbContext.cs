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
using log4net;

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
        #region Log

        /// <summary>
        /// The logger to log events
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(DynamicDbContext));

        #endregion Log

        #region Properties

        /// <summary>
        /// A Database factory instance which is used to create all database objects
        /// </summary>
        public IDbFactory DbFactory { get; protected set; }

        /// <summary>
        /// The Database data type context
        /// </summary>
        public IDataTypeContext DbDataType { get; protected set; }

        /// <summary>
        /// The connection object
        /// </summary>
        public IDynamicDbConnection DbConnection { get; protected set; }

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
        public DynamicDbContext() : this(new DynamicDbFactory()) { }

        /// <summary>
        /// Constructor which provides its own factory class
        /// </summary>
        public DynamicDbContext(IDbFactory dbfactory)
        {
            DbFactory = dbfactory;
            DbConnection = DbFactory.Create<IDynamicDbConnection>();
            DbCommand = DbFactory.Create<IDbCommand>();

            logger.Debug("New instance created");
        }

        /// <summary>
        /// Initialize with db database file and name
        /// </summary>
        /// <param name="arg">The argument to initialize conext object</param>
        public virtual void Initialize(ArgsContextInitialize arg)
        {
            if (arg == null) throw new Exception("The context initialize argument is null");
            DbConnection.Initialize(arg.DbFilePath, arg.Name, arg.ConnectionString);
            DbCommand.Initialize(this, DbConnection);

            logger.Debug("The Db connection object and the Db command object is initialized");
        }

        #endregion Initialize

        #region Database

        /// <summary>
        /// An empty virtual method whose purpose is to create database
        /// </summary>
        public virtual void CreateDatabase()
        {
            DbConnection.CreateDatabase();

            logger.Debug("A new database is created");
        }
        
        /// <summary>
        /// An empty virtual method whose purpose is to check if the database already exists or not
        /// </summary>
        /// <returns></returns>
        public virtual bool DatabaseExists()
        {
            return DbConnection.DatabaseExists();
        }

        /// <summary>
        /// An empty virtual method whose purpose is to delete database
        /// </summary>
        public virtual void DeleteDatabase()
        {
            DbConnection.DeleteDatabase();

            logger.DebugFormat("{0} Database is deleted", DbConnection.DbName);
        }

        #endregion Database

        #region Connection

        /// <summary>
        /// Use this method to open connection to the database
        /// </summary>
        public virtual void Open()
        {
            DbConnection.Open();

            logger.DebugFormat("{0} Database connectivity is opened", DbConnection.DbName);
        }

        /// <summary>
        /// Use this method to close connection to the database
        /// </summary>
        public virtual void Close()
        {
            DbConnection.Close();

            logger.DebugFormat("{0} Database connectivity is closed", DbConnection.DbName);
        }

        #endregion Connection

        #region Create

        /// <summary>
        /// Create multiple tables from the dynamic config data
        /// </summary>
        public virtual void CreateTable(
            Dictionary<string, Dictionary<string, ConfigDbColumn>> tableColumnConfigs)
        {
            if (tableColumnConfigs == null) return;
            logger.DebugFormat("Create {0} tables from ConfigDbColumn", tableColumnConfigs.Count);

            // Create App specific tables
            foreach (var kv in tableColumnConfigs)
                CreateTable(kv.Key, kv.Value);
        }

        /// <summary>
        /// Create a new table in the database from column config. If the table exists then throw error.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="configCols">The table column configurations to create the table</param>
        public virtual void CreateTable(string tableName, Dictionary<string, ConfigDbColumn> configCols)
            => CreateTable(tableName, (dynTable) => dynTable.CreateTable(configCols));

        /// <summary>
        /// Delete a table
        /// </summary>
        /// <param name="tablename">The name of the table</param>
        public virtual void DeleteTable(string tablename)
        {
            if (string.IsNullOrEmpty(tablename)) throw new Exception("No table name provided to delete");
            if (Tables != null && Tables.Count > 0 && Tables.ContainsKey(tablename))
            {
                logger.DebugFormat("Table {0} is loaded in memory. Clearing memory data ...", tablename);
                IDbTable table = GetTable(tablename);
                table.Delete();
                Tables.Remove(tablename);
            }
            
            // Finally delete from the database
            DbCommand.RemoveTable(tablename);
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
            IDbTable dynTable = DbFactory.Create<IDbTable>();
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
        /// Use this method to load data
        /// Load the table data and metdata from the database and sort by the unique keys column names
        /// This method is useful for inserting or updating rows by these unique columns
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cols">The list of unique keys columns</param>
        public virtual void Load(string name, params string[] cols)
        {
            LoadMetadata(name);
            Load(name, (dynTable) => {
                dynTable.LoadData(DbCommand.LoadData(name), cols);
            });
        }

        /// <summary>
        /// Load the metadata of the table.
        /// </summary>
        /// <param name="name">The name of the table</param>
        public virtual void LoadMetadata(string tablename)
            => Load(tablename, (dynTable) => {
                dynTable.LoadTableMetadata(DbCommand.LoadTableMetadata(tablename));
            });

        /// <summary>
        /// Load the table data. This method is not public. We should always use <see cref="Load(string)"/>
        /// to load data into a table as Loading of metdata alongwith data is important
        /// </summary>
        /// <param name="name"></param>
        protected void LoadData(string tablename)
            => Load(tablename, (dynTable) => {
                dynTable.LoadData(DbCommand.LoadData(tablename));
            });
        
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
        public void Clear(string tablename) => GetTable(tablename).Clear();
        
        #endregion Load

        #region Insert

        /// <summary>
        /// Commit the chnages to the database
        /// </summary>
        public void Commit(string tableName)
        {
            if (!Tables.ContainsKey(tableName)) throw new Exception(tableName + " table not created or not found");
            DbCommand.InsertOrReplace(Tables[tableName]);
        }

        /// <summary>
        /// Insert into the table. Data is indexed by column
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colIndexData"></param>
        void AddOrUpdate(string tableName, string[] colIndexData)
        {
            if (!Tables.ContainsKey(tableName)) Load(tableName);
            if (!Tables.ContainsKey(tableName)) throw new Exception(tableName + " table not created or not found");

            Tables[tableName].AddorUpdate(colIndexData);
            DbCommand.Insert(Tables[tableName], colIndexData);
        }

        /// <summary>
        /// Add or update a table row by the given table name.
        /// Find the row using the unique keys column names provided.
        /// The 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ukeycolNames"></param>
        /// <param name="row"></param>
        public virtual void AddOrUpdate(string tablename, IEnumerable<string> ukeycolNames,
            IDictionary<string, DbDataType> row)
            => AddOrUpdate(tablename, (table) => table.AddorUpdate(ukeys, row));
        
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
        /// A common helper method to execute common set of Add or update methods
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fnAddorUpdate"></param>
        protected void AddOrUpdate(string tablename, Action<IDbTable> fnAddorUpdate)
            => fnAddorUpdate(GetTable(tablename));

        /// <summary>
        /// Bulk add the table metadata information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tableMetas"></param>
        public virtual void AddOrUpdate(string name, Dictionary<string, ConfigDbTable> tableMetas)
        {
            if (!Tables.ContainsKey(name)) Load(name);
            if (!Tables.ContainsKey(name)) throw new Exception(name + " table not created or not found");

            Tables[name].AddorUpdate(tableMetas);
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
            if (Tables == null || Tables.Count <= 0) throw new Exception("No table is loaded yet into memory");
            if (!Tables.ContainsKey(tablename)) throw new Exception(tablename + ": No such table found");
            return Tables[tablename];
        }

        #endregion Helper
        
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(Tables != null) foreach (var item in Tables) item.Value.Dispose();

                    if (DbCommand != null) DbCommand.Dispose();
                    if (DbConnection != null) DbConnection.Dispose();
                    if (DbFactory != null) DbFactory = null;
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
