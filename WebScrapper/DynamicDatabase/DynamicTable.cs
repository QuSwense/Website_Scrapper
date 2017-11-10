﻿using DynamicDatabase.Config;
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
    /// This class is part of the<see cref="IDbContext"/> type classes. This class has no use as a seperate
    /// entity. It keeps a reference to the parent Context class.
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
        public bool IsDirty { get; set; }

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

        #region Create

        /// <summary>
        /// Commit the whole table to the database if the Dirty flag is set including create and insert
        /// </summary>
        public void Commit()
        {
            if(IsDirty)
            {
                DbContext.DbCommand.CreateTable(this);
                Commited();
            }

            if(Rows != null) Rows.Commit();
        }

        /// <summary>
        /// Loop through the column configuration and create a new table
        /// </summary>
        /// <param name="configCols"></param>
        public void CreateTable(DbColumnsDefinitionModel configCols)
        {
            // Reinitialize headers. It will destroy the previous loaded data
            Headers = DbContext.DbFactory.Create<IColumnHeaders>();
            Headers.Initialize(this, configCols);
            IsDirty = true;
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
            Headers.Initialize(this, reader);
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

            while (reader.Read()) Rows.AddorUpdate(reader);
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
                    row = Rows.NewRow(this);
                    row.AddorUpdate(0, rowToInsert.Key);
                }

                row.AddorUpdate(1, rowToInsert.Value.Display);
                Rows.AddorUpdate(row);
            }
        }

        /// <summary>
        /// Insert into the table. Data is indexed by column id
        /// </summary>
        /// <param name="rowData"></param>
        public void AddOrUpdate(string[] rowData)
        {
            if (Rows == null) Rows = new DynamicRows(this);

            string ukeyString = DynamicDbHelper.GetPrimaryKeyString(GetUniqueKeys(rowData));

            var row = Rows.FindByPK(ukeyString);

            if(row == null) row = Rows.NewRow(this);

            for (int i = 0; i < rowData.Length; ++i) row.AddorUpdate(i, rowData[i]);
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

            if (row == null) row = Rows.NewRow(this);

            foreach (var kv in dataList) row.AddorUpdate(kv.Key, kv.Value);

            return row.ToStringByPK();
        }

        #endregion Insert

        #region Helper

        /// <summary>
        /// Get the index from the column name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetColumnIndex(string name) => Headers.GetColumnIndex(name);

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

        /// <summary>
        /// The table is saved in the database.
        /// It doesnt mean that the data in the table is saved yet
        /// </summary>
        /// <returns></returns>
        public void Commited()
        {
            IsDirty = false;
        }

        /// <summary>
        /// Get a list of unique keys for the row
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private List<string> GetUniqueKeys(string[] rowData)
        {
            List<string> ukeys = new List<string>();
            for (int i = 0; i < Headers.Count(); ++i)
                if (Headers[i].IsPK) ukeys.Add(rowData[i]);

            return ukeys;
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
                    if (Rows != null) foreach (var item in Rows) item.Dispose();
                    if (Headers != null) foreach (var item in Headers) item.Dispose();
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
        }

        #endregion
    }
}
