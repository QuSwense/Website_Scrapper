using DynamicDatabase.Config;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
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
        /// The rows in the table. The data key is RowId.
        /// </summary>
        List<IDbRow> Rows { get; }

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

        /// <summary>
        /// Get the index from the column name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetColumnIndex(string name);

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

        /// <summary>
        /// Get the list of Primary keys by name
        /// </summary>
        /// <returns></returns>
        List<string> GetPKNames();

        #region Insert
        
        /// <summary>
        /// Add rows of metadata table
        /// </summary>
        /// <param name="tableMetas"></param>
        void AddorUpdate(Dictionary<string, ConfigDbTable> tableMetas);

        /// <summary>
        /// Add rows of data
        /// </summary>
        /// <param name="row"></param>
        void AddorUpdate(List<TableDataColumnModel> row);

        /// <summary>
        /// Add row of data
        /// </summary>
        /// <param name="row"></param>
        void AddorUpdate(List<string> pks, List<string> dataList);

        #endregion Insert
    }
}
