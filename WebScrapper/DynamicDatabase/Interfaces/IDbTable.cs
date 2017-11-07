using DynamicDatabase.Config;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using DynamicDatabase.Types;

namespace DynamicDatabase.Interfaces
{
    /// <summary>
    /// An interface to the <see cref="DynamicTable{TDynRow, TDynColMetadata}"/> class.
    /// It is used as a way to call the methods of the class mostly for back reference.
    /// </summary>
    public interface IDbTable : IDisposable
    {
        #region Properties

        /// <summary>
        /// Reference to the parent database context
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// The name of the table
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// The rows of the table
        /// </summary>
        DynamicRows Rows { get; }

        /// <summary>
        /// The list of column headers
        /// </summary>
        IColumnHeaders Headers { get; }

        #endregion Properties

        #region Initialize

        /// <summary>
        /// Constructor with table name
        /// </summary>
        /// <param name="tablename"></param>
        void Initialize(IDbContext dbContext, string tablename);

        #endregion Initialize

        #region Helper

        /// <summary>
        /// Get the index from the column name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetColumnIndex(string name);

        #endregion Helper

        #region Create

        /// <summary>
        /// Create table from property type (soft create in memory)
        /// </summary>
        /// <param name="classProperties"></param>
        void CreateTable(PropertyInfo[] classProperties);

        /// <summary>
        /// Loop through the column configuration and create a new table
        /// </summary>
        /// <param name="configCols"></param>
        void CreateTable(Dictionary<string, ConfigDbColumn> configCols);

        /// <summary>
        /// Cleanup and soft delete the current table
        /// </summary>
        void Delete();

        #endregion Create

        #region Load

        /// <summary>
        /// Load table metadata. This is the metadata query result
        /// </summary>
        /// <param name="reader"></param>
        void LoadTableMetadata(DbDataReader reader);

        /// <summary>
        /// Load data in memory by Rowid
        /// </summary>
        /// <param name="reader"></param>
        void LoadData(DbDataReader reader);

        /// <summary>
        /// Load data in memory by Rowid
        /// </summary>
        /// <param name="reader"></param>
        void LoadData<T>(DbDataReader reader);

        /// <summary>
        /// Use this method to load data
        /// Load the table data and metdata from the database
        /// </summary>
        /// <param name="name"></param>
        void LoadData(DbDataReader reader, params string[] args);

        /// <summary>
        /// Clear all table data
        /// </summary>
        void Clear();

        #endregion Load

        #region Insert

        /// <summary>
        /// Add or update a row using the unique keys.
        /// For this method to work it is mandatory that the tbale class is registered before with 
        /// <see cref="DynamicSortTable"/>
        /// </summary>
        /// <param name="ukeys"></param>
        /// <param name="row"></param>
        void AddOrUpdate(IEnumerable<DbDataType> ukeys, IEnumerable<DbDataType> row);

        /// <summary>
        /// Add or update a row using the the unique keys with column names.
        /// </summary>
        /// <param name="ukeys">The unique keys which is used to insert the data into table.</param>
        /// <param name="row">The row data to insert into table indexed by zero.</param>
        void AddOrUpdate(IDictionary<string, DbDataType> ukeys, IEnumerable<DbDataType> row);

        /// <summary>
        /// Add or update a row using the the unique keys with column names.
        /// </summary>
        /// <param name="ukeys">The unique keys which is used to insert the data into table.</param>
        /// <param name="row">The row data to insert into table indexed by column name.</param>
        void AddOrUpdate(IDictionary<string, DbDataType> ukeys, IDictionary<string, DbDataType> row);

        /// <summary>
        /// Add rows of metadata table
        /// </summary>
        /// <param name="tableMetas"></param>
        void AddorUpdate(Dictionary<string, ConfigDbTable> tableMetas);

        /// <summary>
        /// Add row of data
        /// </summary>
        /// <param name="row"></param>
        void AddorUpdate(IEnumerable<string> pks, IEnumerable<string> dataList);

        #endregion Insert

        #region Helper

        /// <summary>
        /// Get the list of Primary keys by name
        /// </summary>
        /// <returns></returns>
        List<string> GetPKNames();

        #endregion Helper
    }
}
