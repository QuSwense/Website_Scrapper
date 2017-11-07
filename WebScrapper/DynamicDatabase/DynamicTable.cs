using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Model;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

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

        /// <summary>
        /// Set the dirty flag for a new insert or update
        /// </summary>
        public bool IsDirty { get; protected set; }

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
            IsDirty = false;
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
        public void CreateTable(DbColumnsDefinitionModel configCols)
        {
            // Reinitialize headers. It will destroy the previous loaded data
            Headers = DbContext.DbFactory.Create<IColumnHeaders>();
            Headers.Initialize(configCols);
        }

        /// <summary>
        /// Cleanup and soft delete the current table
        /// </summary>
        public void Delete()
        {
            Headers = null;
            Rows = null;
        }

        #endregion Create

        #region Load
        
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
            Headers.Dispose();
            Rows = null;
        }

        #endregion Load

        #region Insert

        /// <summary>
        /// Add or update tables definition data to the table metdata.
        /// Use this for Table Metadata update
        /// </summary>
        /// <param name="name">The table name</param>
        /// <param name="metadataModel"></param>
        public void AddOrUpdate(DbTablesMetdataDefinitionModel metadataModel)
        {
            if (Rows == null) Rows = new DynamicRows(this);

            foreach (var rowToInsert in metadataModel)
            {
                var row = Rows.FindByPK(rowToInsert.Key);
                if (row == null)
                {
                    row = DbContext.DbFactory.Create<IDbRow>();
                    row.Initialize(this);
                    row.Columns[0].Value = rowToInsert.Key;
                }

                row.Columns[1].Value = rowToInsert.Value.Display;
            }
        }

        /// <summary>
        /// Insert into the table. Data is indexed by column
        /// </summary>
        /// <param name="colIndexData"></param>
        public void AddOrUpdate(string[] colIndexData)
        {
            List<string> ukeys = new List<string>();
            for(int i = 0; i < Headers.ByIndices.Count; ++i)
            {
                if (Headers.ByIndices[i].IsPK) ukeys.Add(colIndexData[i]);
            }

            string ukeyString = DynamicDbHelper.GetPrimaryKeyString(ukeys);

            var row = Rows.FindByPK(ukeyString);

            if(row == null)
            {
                row = DbContext.DbFactory.Create<IDbRow>();
                row.Initialize(this);
            }

            for(int i = 0; i < colIndexData.Length; ++i)
            {
                row.AddorUpdate(i, colIndexData[i]);
            }
        }

        /// <summary>
        /// Insert into the table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colIndexData"></param>
        /// <param name="dataList"></param>
        public string AddOrUpdate(List<string> ukeys, Dictionary<string, string> dataList)
        {
            if (Rows == null) Rows = new DynamicRows(this);
            var row = Rows.FindByPK(DynamicDbHelper.GetPrimaryKeyString(ukeys));

            if (row == null)
            {
                row = DbContext.DbFactory.Create<IDbRow>();
                row.Initialize(this);
            }

            foreach (var kv in dataList)
            {
                row.AddorUpdate(kv.Key, kv.Value);
            }

            return row.ToStringByPK();
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
