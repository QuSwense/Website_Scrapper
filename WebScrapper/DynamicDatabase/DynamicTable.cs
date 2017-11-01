using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Model;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicDatabase
{
    /// <summary>
    /// Represents a Database table.
    /// </summary>
    /// <typeparam name="TDynRow"></typeparam>
    /// <typeparam name="TDynColMetadata"></typeparam>
    /// <typeparam name="TDynCol"></typeparam>
    public class DynamicTable : IDbTable
    {
        #region Properties

        /// <summary>
        /// Reference to the parent database context
        /// </summary>
        public IDbContext DbContext { get; protected set; }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// The list of column headers
        /// </summary>
        public IColumnHeaders Headers { get; protected set; }

        /// <summary>
        /// The rows of the table
        /// </summary>
        public DynamicRows Rows { get; protected set; }

        #endregion Properties

        #region Initialize

        /// <summary>
        /// Constructor default
        /// </summary>
        public DynamicTable() { }

        /// <summary>
        /// Constructor with table name
        /// </summary>
        /// <param name="tablename"></param>
        public virtual void Initialize(IDbContext dbContext, string tablename)
        {
            DbContext = dbContext;
            TableName = tablename;
        }

        #endregion Initialize

        #region Helper

        /// <summary>
        /// Get the index from the column name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetColumnIndex(string name) => Headers.GetColumnIndex(name);

        #endregion Helper

        #region Create

        /// <summary>
        /// Loop through the column configuration and create a new table
        /// </summary>
        /// <param name="configCols"></param>
        public void CreateTable(Dictionary<string, ConfigDbColumn> configCols)
        {
            // Reinitialize headers. It will destroy the previous loaded data
            Headers = DbContext.DbFactory.Create<IColumnHeaders>();
            Headers.Initialize(configCols);
        }

        /// <summary>
        /// Create table from property type (soft create in memory)
        /// </summary>
        /// <param name="classProperties"></param>
        public void CreateTable(PropertyInfo[] classProperties)
        {
            // Reinitialize headers. It will destroy the previous loaded data
            Headers = DbContext.DbFactory.Create<IColumnHeaders>();
            Headers.Initialize(classProperties);
        }

        #endregion Create

        #region Load

        /// <summary>
        /// Load the data from the table into memory and also initialize the data set for
        /// unique key navigation
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="cols"></param>
        public virtual void LoadData(DbDataReader reader, params string[] cols)
        {
            if (Rows == null) Rows = new DynamicRows(this);

            while(reader.Read())
            {
                Rows.Load(reader, cols);
            }
        }

        /// <summary>
        /// Load table metadata. This is the metadata query result
        /// </summary>
        /// <param name="reader"></param>
        public void LoadTableMetadata(DbDataReader reader)
        {
            // Reinitialize headers. It will destroy the previous loaded data
            Headers = DbContext.DbFactory.Create<IColumnHeaders>();
            Headers.Initialize(reader);
        }

        /// <summary>
        /// Load data in memory by Rowid
        /// </summary>
        /// <param name="reader"></param>
        public virtual void LoadData(DbDataReader reader)
        {
            if (Headers == null) throw new Exception("Table metadata must be loaded before data");

            // Destroys previous data if exists
            Rows = new DynamicRows(this);

            while (reader.Read()) Rows.AddOrUpdate(reader);
        }

        /// <summary>
        /// Clear all table data
        /// </summary>
        public void Clear()
        {

        }

        #endregion Load

        #region Insert

        /// <summary>
        /// Add or update a row using the unique key columns.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <param name="ukeys"></param>
        /// <param name="row"></param>
        public virtual void AddOrUpdate(IEnumerable<DbDataType> ukeys, IEnumerable<DbDataType> row)
        {
            if (Rows == null) Rows = new DynamicRows(this);

            // Get the unique key columns
            string ukeyString = DynamicDbHelper.GetPrimaryKeyString(ukeys);
        }

        /// <summary>
        /// Add or update a row using the the unique keys with column names.
        /// </summary>
        /// <param name="ukeys">The unique keys which is used to insert the data into table.</param>
        /// <param name="row">The row data to insert into table indexed by zero.</param>
        public virtual void AddOrUpdate(IDictionary<string, DbDataType> ukeys, IEnumerable<DbDataType> row)
        {
            string ukeydata = string.Join(",", ukeys.Values);
            List<string> colNames = ukeys.Keys.ToList();
        }

        /// <summary>
        /// Add or update a row using the the unique keys with column names.
        /// </summary>
        /// <param name="ukeys">The unique keys which is used to insert the data into table.</param>
        /// <param name="row">The row data to insert into table indexed by column name.</param>
        public virtual void AddOrUpdate(IDictionary<string, DbDataType> ukeys, IDictionary<string, DbDataType> row)
        {

        }

        /// <summary>
        /// Add rows of metadata table
        /// </summary>
        /// <param name="tableMetas"></param>
        public void AddorUpdate(Dictionary<string, ConfigDbColumn> tableColMetas)
        {
            foreach (var item in tableColMetas)
            {
                var row = Rows.FindByPK(item.Key);
                if (row == null)
                {
                    row = DbContext.DbFactory.Create<IDbRow>();
                    row.Initialize(this);
                    row.Columns[0].Value = item.Key;
                }

                row.Columns[1].Value = item.Value.Display;
            }
        }

        #endregion Insert

        /// <summary>
        /// Get the list of Primary keys by name
        /// </summary>
        /// <returns></returns>
        public List<string> GetPKNames()
        {
            List<IColumnMetadata> pkColumns = Headers.GetPKs();

            if (pkColumns != null || pkColumns.Count > 0)
                return pkColumns.Select(p => p.ColumnName).ToList();
            else
                return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Rows != null)
                    {
                        foreach (var item in Rows)
                        {
                            item.Dispose();
                        }
                    }

                    if (Headers != null)
                    {
                        foreach (var item in Headers)
                        {
                            item.Dispose();
                        }
                    }
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
