using System;
using System.Collections.Generic;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Config;
using DynamicDatabase.Meta;
using WebCommon.Extn;
using DynamicDatabase.Model;
using DynamicDatabase.Types;
using log4net;
using WebCommon.Error;

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
        /// Static initializer
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static IDbContext Init(ArgsContextInitialize arg, string dbType)
        {
            IDbContext dbContext = new DynamicDbContext();
            dbContext.Initialize(arg);
            dbContext.DbFactory.RegisterDb(dbType);
            return dbContext;
        }

        /// <summary>
        /// Initialize with db database file and name
        /// </summary>
        /// <param name="arg">The argument to initialize conext object</param>
        public virtual void Initialize(ArgsContextInitialize arg)
        {
            if (arg == null) throw new Exception("The context initialize argument is null");
            DbConnection.Initialize(arg.DbFilePath, arg.Name, arg.ConnectionString);
            DbCommand.Initialize(this);

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
        /// <param name="tableColumnConfigs"></param>
        public virtual void CreateTable(DbTablesDefinitionModel tableColumnConfigs)
        {
            if (tableColumnConfigs == null) return;
            logger.DebugFormat("Create {0} tables from ConfigDbColumn", tableColumnConfigs.Count);

            // Create all tables
            foreach (var kv in tableColumnConfigs)
                CreateTable(kv.Key, kv.Value);
        }

        /// <summary>
        /// Create a new table in the database from column config. If the table exists then throw error.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="configCols">The table column configurations to create the table</param>
        public virtual void CreateTable(string tableName, DbColumnsDefinitionModel configCols)
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
        /// Commit all the changes to the database
        /// This uses the IsDirty flag of <see cref="IDbTable"/> and <see cref="IDbRow"/> to 
        /// check if the Table or row needs to be created / updated
        /// </summary>
        public void Commit()
        {
            foreach (var table in Tables) Commit(table.Key);
        }

        /// <summary>
        /// Commit the table change to the database
        /// This uses the IsDirty flag of <see cref="IDbTable"/> and <see cref="IDbRow"/> to 
        /// check if the Table or row needs to be created / updated
        /// </summary>
        public void Commit(string tableName)
        {
            // if the table is not loaded in memory then there is no update
            if (!Tables.ContainsKey(tableName)) return;

            if (Tables[tableName].IsDirty)
                DbCommand.CreateTable(Tables[tableName]);
            else
                DbCommand.InsertOrReplace(Tables[tableName]);
        }

        /// <summary>
        /// Add or update tables definition data to the table metdata
        /// </summary>
        /// <param name="name">The table name</param>
        /// <param name="metadataModel"></param>
        public void AddOrUpdate(string name, DbTablesMetdataDefinitionModel metadataModel)
        {
            FindAndLoadTable(name);
            Tables[name].AddOrUpdate(metadataModel);
        }

        /// <summary>
        /// Insert into the table. Data is indexed by column
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colIndexData"></param>
        public void AddOrUpdate(string tableName, string[] colIndexData)
        {
            FindAndLoadTable(tableName);
            Tables[tableName].AddOrUpdate(colIndexData);
        }

        /// <summary>
        /// Insert into the table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colIndexData"></param>
        public string AddOrUpdate(string tableName, List<string> ukeys, Dictionary<string, string> dataList)
        {
            FindAndLoadTable(tableName);
            return Tables[tableName].AddOrUpdate(ukeys, dataList);
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

        /// <summary>
        /// Find the table
        /// </summary>
        /// <param name="tableName"></param>
        protected void FindAndLoadTable(string tableName)
        {
            if (!Tables.ContainsKey(tableName)) Load(tableName);
            if (!Tables.ContainsKey(tableName))
                throw new DynamicDbException(tableName, DynamicDbException.EErrorType.TABLE_NOT_FOUND);
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
