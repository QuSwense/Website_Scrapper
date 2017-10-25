using DynamicDatabase.Config;
using System;
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
        /// <summary>
        /// The connection object
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

        /// <summary>
        /// Use this method to open connection to the database
        /// </summary>
        void Open();

        /// <summary>
        /// Use this method to close connection to the database
        /// </summary>
        void Close();

        /// <summary>
        /// Create a new table in the database using a class type. That class should be decorated with 
        /// Database attributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void CreateTable<T>();

        /// <summary>
        /// Create the database from the config file dynamically
        /// </summary>
        void CreateTable(
            Dictionary<string, Dictionary<string, ConfigDbColumn>> TableColumnConfigs);

        /// <summary>
        /// Create a new table in the database. If the table exists then throw error.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="configCols">The table column configurations to create the table</param>
        void CreateTable(string tableName,
            Dictionary<string, ConfigDbColumn> configCols);

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
        void LoadMetadata(string name);

        /// <summary>
        /// This is a virtual method which needs to be overriden in the derived class.
        /// Different Databse will have different data types
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        string GetDataType(Type propertyType);
    }
}
