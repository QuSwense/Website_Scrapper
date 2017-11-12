using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using DynamicDatabase.Config;
using System.Collections;
using DynamicDatabase.Interfaces;
using System.Linq;
using WebCommon.Error;
using System.Diagnostics;

namespace DynamicDatabase
{
    public class DynamicColumnHeaders : IColumnHeaders,
        IEnumerator<IColumnMetadata>,
        IEnumerable<IColumnMetadata>
    {
        #region Properties

        /// <summary>
        /// Refers to the parent table
        /// </summary>
        public IDbTable Table { get; protected set; }

        /// <summary>
        /// The list of column headers by name
        /// </summary>
        public Dictionary<string, IColumnMetadata> ByNames { get; protected set; }

        /// <summary>
        /// The list of column headers by index
        /// </summary>
        public List<IColumnMetadata> ByIndices { get; protected set; }

        #endregion Properties

        #region Properties Helper

        /// <summary>
        /// Get the Database factory
        /// </summary>
        protected IDbFactory DbFactory
        {
            get { return Table.DbContext.DbFactory; }
        }

        #endregion

        #region Indexer

        /// <summary>
        /// An indexer to access data like array using index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IColumnMetadata this[int index]
        {
            get
            {
                if (ByIndices == null || ByIndices.Count <= index)
                    throw new IndexOutOfRangeException("Column Index out of range");
                return ByIndices[index];
            }
            set
            {
                if (ByIndices == null || ByIndices.Count <= index)
                    throw new IndexOutOfRangeException("Column Index out of range");
                ByIndices[index] = value;
            }
        }

        /// <summary>
        /// An indexer to access data like array using column name
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IColumnMetadata this[string name]
        {
            get
            {
                if (ByNames == null || !ByNames.ContainsKey(name))
                    throw new IndexOutOfRangeException("Column name do not exists");
                return ByNames[name];
            }
            set
            {
                if (ByNames == null || !ByNames.ContainsKey(name))
                    throw new IndexOutOfRangeException("Column name do not exists");
                ByNames[name] = value;
            }
        }

        #endregion Indexer

        #region Constructor

        /// <summary>
        /// Initialize the column headers using config
        /// </summary>
        /// <param name="configCols"></param>
        public void Initialize(IDbTable tableParent, Dictionary<string, ConfigDbColumn> configCols)
        {
            Table = tableParent;
            foreach (var item in configCols)
            {
                IColumnMetadata colMetadata = DbFactory.CreateColumnMetadata(Table);
                colMetadata.Parse(item.Key, item.Value);
                AddHeader(colMetadata);
            }
        }

        /// <summary>
        /// Initialize from the metdata reader
        /// </summary>
        /// <param name="reader"></param>
        public void Initialize(IDbTable tableParent, DbDataReader reader)
        {
            Table = tableParent;
            while (reader.Read())
            {
                IColumnMetadata colMetadata = DbFactory.CreateColumnMetadata(Table);
                colMetadata.Parse(reader);
                AddHeader(colMetadata);
            }
        }

        /// <summary>
        /// Initialize from the metdata reader with partial columns
        /// </summary>
        /// <param name="reader"></param>
        public void Initialize(IDbTable tableParent, DbDataReader reader, 
            Dictionary<string, ColumnLoadDataModel> columns)
        {
            Table = tableParent;
            while (reader.Read())
            {
                IColumnMetadata colMetadata = DbFactory.CreateColumnMetadata(Table);
                colMetadata.Parse(reader);
                
                if (columns.ContainsKey(colMetadata.ColumnName))
                {
                    colMetadata.Merge(columns[colMetadata.ColumnName]);
                    AddHeader(colMetadata);
                }
            }
        }

        #endregion Constructor

        #region Utility

        /// <summary>
        /// Get the index of the column
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetColumnIndex(string name)
        {
            Debug.Assert(!ByNames.ContainsKey(name));
            Debug.Assert(ByNames[name].Index < 0 || ByNames[name].Index > ByIndices.Count);

            if (!ByNames.ContainsKey(name) ||
                ByNames[name].Index < 0 || ByNames[name].Index > ByIndices.Count) return -1;
            return ByNames[name].Index;
        }

        /// <summary>
        /// Get the list of PKs
        /// </summary>
        /// <returns></returns>
        public List<IColumnMetadata> GetPKs() => ByIndices.Where(p => p.IsPK).ToList();

        /// <summary>
        /// Get the list of PKs
        /// </summary>
        /// <returns></returns>
        public List<IColumnMetadata> GetUKs() => ByIndices.Where(p => p.IsUnique).ToList();

        #endregion Utility

        #region Insert

        /// <summary>
        /// Add header to the table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dynCol"></param>
        protected void AddHeader(IColumnMetadata dynCol)
        {
            if (ByNames == null) ByNames = new Dictionary<string, IColumnMetadata>();
            ByNames.Add(dynCol.ColumnName, dynCol);

            if (ByIndices == null) ByIndices = new List<IColumnMetadata>();
            ByIndices.Add(dynCol);
        }

        /// <summary>
        /// Add header to the table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dynCol"></param>
        protected void AddHeader(int index, string key, IColumnMetadata dynCol)
        {
            if (ByNames == null) ByNames = new Dictionary<string, IColumnMetadata>();
            ByNames.Add(key, dynCol);

            if (ByIndices == null) ByIndices = new List<IColumnMetadata>();
            ByIndices.Insert(index, dynCol);
        }

        #endregion Insert

        #region IEnumerator
        private int _position = -1;

        public IColumnMetadata Current
        {
            get
            {
                return ByIndices[_position];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return ByIndices[_position];
            }
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < ByIndices.Count);
        }

        public void Reset()
        {
            _position = 0;
        }
        
        IEnumerator<IColumnMetadata> GetEnumerator()
        {
            return ByIndices.GetEnumerator();
        }

        IEnumerator<IColumnMetadata> IEnumerable<IColumnMetadata>.GetEnumerator()
        {
            return ByIndices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ByIndices.GetEnumerator();
        }

        #endregion IEnumerator

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
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
