using DynamicDatabase.Config;
using DynamicDatabase.Model;
using DynamicDatabase.Types;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicDatabase.Interfaces
{
    /// <summary>
    /// The database context interface
    /// </summary>
    public interface IDbContext : IDisposable
    {
        #region Properties

        /// <summary>
        /// A Database factory instance which is used to create all database objects
        /// </summary>
        IDbFactory DbFactory { get; }

        /// <summary>
        /// The Database data type context
        /// </summary>
        IDataTypeContext DbDataType { get; }

        /// <summary>
        /// The connection object
        /// </summary>
        IDynamicDbConnection DbConnection { get; }

        /// <summary>
        /// The list of all tables in the database
        /// </summary>
        Dictionary<string, IDbTable> Tables { get; }

        /// <summary>
        /// The object which builds and execute any database query of rthe database context.
        /// To retrieve the current sql query executed in the database use the <see cref="SQL"/>
        /// property
        /// </summary>
        IDbCommand DbCommand { get; }

        #endregion Properties

        #region Initialize

        /// <summary>
        /// Initialize with db database file and name
        /// </summary>
        /// <param name="arg">The argument to initialize conext object</param>
        void Initialize(ArgsContextInitialize arg);

        #endregion Initialize

        #region Database

        /// <summary>
        /// An empty virtual method whose purpose is to create database
        /// </summary>
        void CreateDatabase();

        /// <summary>
        /// An empty virtual method whose purpose is to check if the database already exists or not
        /// </summary>
        /// <returns></returns>
        bool DatabaseExists();

        /// <summary>
        /// An empty virtual method whose purpose is to delete database
        /// </summary>
        void DeleteDatabase();

        #endregion Database

        #region Connection

        /// <summary>
        /// Use this method to open connection to the database
        /// </summary>
        void Open();

        /// <summary>
        /// Use this method to close connection to the database
        /// </summary>
        void Close();

        #endregion Connection

        #region Create

        /// <summary>
        /// Create multiple tables from the dynamic config data
        /// </summary>
        /// <param name="tableColumnConfigs"></param>
        void CreateTable(DbTablesDefinitionModel tableColumnConfigs);

        /// <summary>
        /// Create a new table in the database from column config. If the table exists then throw error.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="configCols">The table column configurations to create the table</param>
        void CreateTable(string tableName, DbColumnsDefinitionModel configCols);

        /// <summary>
        /// Delete a table
        /// </summary>
        /// <param name="tablename">The name of the table</param>
        void DeleteTable(string tablename);

        #endregion Create

        #region Load

        /// <summary>
        /// Use this method to load data
        /// Load the table data and metdata from the database
        /// </summary>
        /// <param name="name"></param>
        void Load(string name);
        
        /// <summary>
        /// Load the metadata of the table.
        /// </summary>
        /// <param name="name">The name of the table</param>
        void LoadMetadata(string tablename);

        /// <summary>
        /// Clear the table data that is loaded in memory
        /// </summary>
        /// <param name="name"></param>
        void Clear(string tablename);

        #endregion Load

        #region Insert

        /// <summary>
        /// Commit the chnages to the database
        /// </summary>
        void Commit();

        /// <summary>
        /// Commit the chnages to the database
        /// </summary>
        void Commit(string tableName);

        /// <summary>
        /// Add or update tables definition data to the table metdata
        /// </summary>
        /// <param name="name">The table name</param>
        /// <param name="metadataModel"></param>
        void AddOrUpdate(string name, DbTablesMetdataDefinitionModel metadataModel);

        /// <summary>
        /// Insert into the table. Data is indexed by column
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colIndexData"></param>
        void AddOrUpdate(string tableName, string[] colIndexData);

        /// <summary>
        /// Insert into the table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colIndexData"></param>
        /// <param name="dataList"></param>
        string AddOrUpdate(string tableName, List<string> ukeys, Dictionary<string, string> dataList);
        
        #endregion Insert
    }
}
