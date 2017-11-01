using DynamicDatabase.Config;
using DynamicDatabase.Model;
using DynamicDatabase.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Interfaces
{
    /// <summary>
    /// The database context interface
    /// </summary>
    public interface IDbContext
    {
        #region Properties

        /// <summary>
        /// A Database factory instance which is used to create all database objects
        /// </summary>
        IDbFactory DbFactory { get; }

        /// <summary>
        /// A database data type helper context class. Every database has its own data type.
        /// Use <see cref="DynamicDbFactory"/> to register the Data type class for the database.
        /// </summary>
        IDataTypeContext DataType { get; }

        /// <summary>
        /// The connection object used to help in connecting to the database
        /// </summary>
        IDynamicDbConnection Connection { get; }

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
        /// Constructor with db database file and name
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
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
        /// Create a new table in the database using a class type. That class should be decorated with 
        /// Database attributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void CreateTable<T>();

        /// <summary>
        /// Create multiple tables from the dynamic config data
        /// </summary>
        void CreateTable(Dictionary<string, Dictionary<string, ConfigDbColumn>> tableColumnConfigs);

        /// <summary>
        /// Create a new table in the database from column config. If the table exists then throw error.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="configCols">The table column configurations to create the table</param>
        void CreateTable(string tableName, Dictionary<string, ConfigDbColumn> configCols);

        /// <summary>
        /// Create table(s) using the collection value
        /// </summary>
        /// <param name="colData">The value with column data</param>
        void CreateTable(ICollection colData);

        /// <summary>
        /// Create table(s) using the collection value
        /// </summary>
        /// <param name="name">The table name</param>
        /// <param name="colData">The value with column data</param>
        void CreateTable(string name, ICollection colData);

        /// <summary>
        /// Delete a table
        /// </summary>
        /// <param name="name">The name of the table</param>
        void DeleteTable(string name);

        #endregion Create

        #region Load

        /// <summary>
        /// Use this method to load data
        /// Load the table data and metdata from the database
        /// </summary>
        /// <param name="name"></param>
        void Load(string name);

        /// <summary>
        /// Load the data type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Load<T>();

        /// <summary>
        /// Use this method to load data
        /// Load the table data and metdata from the database and sort by the unique keys column names
        /// This method is useful for inserting or updating rows by these unique columns
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cols">The list of unique keys columns</param>
        void Load(string name, params string[] cols);

        /// <summary>
        /// Load the metadata of the table.
        /// </summary>
        /// <param name="name">The name of the table</param>
        void LoadMetadata(string name);

        /// <summary>
        /// Clear the table data that is loaded in memory
        /// </summary>
        /// <param name="name"></param>
        void Clear(string name);

        #endregion Load

        #region Insert
        
        /// <summary>
        /// Add or update a row using the unique keys.
        /// For this method to work it is mandatory that the tbale class is registered before with 
        /// <see cref="DynamicSortTable"/>
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ukeys"></param>
        /// <param name="row"></param>
        void AddOrUpdate(string tablename, IEnumerable<DbDataType> ukeys, IEnumerable<DbDataType> row);

        /// <summary>
        /// Add or update a row using the the unique keys with column names.
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ukeys">The unique keys which is used to insert the data into table.</param>
        /// <param name="row">The row data to insert into table indexed by zero.</param>
        void AddOrUpdate(string tablename, IDictionary<string, DbDataType> ukeys, IEnumerable<DbDataType> row);

        /// <summary>
        /// Add or update a row using the the unique keys with column names.
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ukeys">The unique keys which is used to insert the data into table.</param>
        /// <param name="row">The row data to insert into table indexed by column name.</param>
        void AddOrUpdate(string tablename, IDictionary<string, DbDataType> ukeys, IDictionary<string, DbDataType> row);

        /// <summary>
        /// Bulk add the table metadata information
        /// </summary>
        /// <param name="tableMetas"></param>
        void AddOrUpdate(Dictionary<string, ConfigDbTable> tableMetas);

        /// <summary>
        /// Bulk add the table metadata information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tableMetas"></param>
        void AddOrUpdate(string name, Dictionary<string, ConfigDbTable> tableMetas);

        #endregion Insert
    }
}
