using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
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
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// The rows in the table. The data key is RowId.
        /// </summary>
        public List<IDbRow> Rows { get; protected set; }

        /// <summary>
        /// The list of column headers
        /// </summary>
        public IColumnHeaders Headers { get; protected set; }

        /// <summary>
        /// Constructor default
        /// </summary>
        public DynamicTable() { }

        /// <summary>
        /// Constructor with table name
        /// </summary>
        /// <param name="tablename"></param>
        public DynamicTable(string tablename)
        {
            TableName = tablename;
        }

        /// <summary>
        /// Get the index from the column name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetColumnIndex(string name)
        {
            return Headers.GetColumnIndex(name);
        }

        /// <summary>
        /// Loop through the column configuration and create a new table
        /// </summary>
        /// <param name="configCols"></param>
        public void CreateTable(Dictionary<string, ConfigDbColumn> configCols)
        {
            // Reinitialize headers. It will destroy the previous loaded data
            Headers = DynamicDbFactory.Create<IColumnHeaders>();
            Headers.Initialize(configCols);
        }

        /// <summary>
        /// Add rows of metadata table
        /// </summary>
        /// <param name="tableMetas"></param>
        public void AddorUpdate(Dictionary<string, ConfigDbTable> tableMetas)
        {
            foreach (var item in tableMetas)
            {
                var row = Rows.Where(r => r.ToStringByPK() == item.Key).First();
                if (row == null) row = DynamicDbFactory.Create<IDbRow>(this);

                row.AddorUpdate(0, item.Key);
                row.AddorUpdate(1, item.Value.Display);
                row.AddorUpdate(2, item.Value.Reference);
            }
        }

        /// <summary>
        /// Create table from property type (soft create in memory)
        /// </summary>
        /// <param name="classProperties"></param>
        public void CreateTable(PropertyInfo[] classProperties)
        {
            // Reinitialize headers. It will destroy the previous loaded data
            Headers = DynamicDbFactory.Create<IColumnHeaders>();
            Headers.Initialize(classProperties);
        }

        /// <summary>
        /// Load table metadata. This is the metadata query result
        /// </summary>
        /// <param name="reader"></param>
        public void LoadTableMetadata(DbDataReader reader)
        {
            // Reinitialize headers. It will destroy the previous loaded data
            Headers = DynamicDbFactory.Create<IColumnHeaders>();
            Headers.Initialize(reader);
        }

        /// <summary>
        /// Load data in memory by Rowid
        /// </summary>
        /// <param name="reader"></param>
        public void LoadData(DbDataReader reader)
        {
            if (Headers == null) throw new Exception("Table metadata must be loaded before data");

            Rows = new List<IDbRow>();

            while (reader.Read())
            {
                var row = DynamicDbFactory.Create<IDbRow>(this);

                foreach(var item in Headers)
                {
                    row.AddorUpdate(item.ColumnName, reader.GetValue(item.Index));
                }
                Rows.Add(row);
            }
        }

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
