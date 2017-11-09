using DynamicDatabase.Config;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using DynamicDatabase.Types;
using DynamicDatabase.Model;

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

        /// <summary>
        /// Set the dirty flag for a new insert or update
        /// </summary>
        bool IsDirty { get; }

        #endregion Properties

        #region Initialize

        /// <summary>
        /// Constructor with table name
        /// </summary>
        /// <param name="tablename"></param>
        void Initialize(IDbContext dbContext, string tablename);

        #endregion Initialize
        
        #region Create

        /// <summary>
        /// Commit the whole table to the database if the Dirty flag is set including create and insert
        /// </summary>
        void Commit();

        /// <summary>
        /// Loop through the column configuration and create a new table
        /// </summary>
        /// <param name="configCols"></param>
        void CreateTable(DbColumnsDefinitionModel configCols);

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
        /// Clear all table data
        /// </summary>
        void Clear();

        #endregion Load

        #region Insert

        /// <summary>
        /// Add or update tables definition data to the table metdata
        /// </summary>
        /// <param name="metadataModel"></param>
        void AddOrUpdate(DbTablesMetdataDefinitionModel metadataModel);

        /// <summary>
        /// Insert into the table. Data is indexed by column
        /// </summary>
        /// <param name="colIndexData"></param>
        void AddOrUpdate(string[] colIndexData);

        /// <summary>
        /// Insert into the table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colIndexData"></param>
        /// <param name="dataList"></param>
        string AddOrUpdate(List<string> ukeys, Dictionary<string, string> dataList);

        #endregion Insert

        #region Helper

        /// <summary>
        /// Get the index from the column name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetColumnIndex(string name);

        /// <summary>
        /// Get the list of Primary keys by name
        /// </summary>
        /// <returns></returns>
        List<string> GetPKNames();

        /// <summary>
        /// The table is saved in the database.
        /// It doesnt mean that the data in the table is saved yet
        /// </summary>
        /// <returns></returns>
        void Commited();

        #endregion Helper
    }
}
