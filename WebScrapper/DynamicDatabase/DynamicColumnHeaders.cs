using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using DynamicDatabase.Config;
using System.Collections;
using DynamicDatabase.Interfaces;

namespace DynamicDatabase
{
    public class DynamicColumnHeaders : IColumnHeaders
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
                if (ByIndices == null || ByIndices.Count <= index) throw new IndexOutOfRangeException("Column Index out of range");
                return ByIndices[index];
            }
            set
            {
                if (ByIndices == null || ByIndices.Count <= index) throw new IndexOutOfRangeException("Column Index out of range");
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
                if (ByNames == null || !ByNames.ContainsKey(name)) throw new IndexOutOfRangeException("Column name do not exists");
                return ByNames[name];
            }
            set
            {
                if (ByNames == null || !ByNames.ContainsKey(name)) throw new IndexOutOfRangeException("Column name do not exists");
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
            int index = 0;
            foreach (var item in configCols)
            {
                IColumnMetadata colMetadata = Table.DbContext.DbFactory.Create<IColumnMetadata>();
                colMetadata.Parse(item.Key, item.Value);
                AddHeader(index, item.Key, colMetadata);
                index++;
            }
        }

        /// <summary>
        /// Initialize from the metdata reader
        /// </summary>
        /// <param name="reader"></param>
        public void Initialize(IDbTable tableParent, DbDataReader reader)
        {
            Table = tableParent;
            int index = 0;
            while (reader.Read())
            {
                IColumnMetadata colMetadata = Table.DbContext.DbFactory.Create<IColumnMetadata>();
                colMetadata.Parse(reader);
                AddHeader(index, colMetadata.ColumnName, colMetadata);
                index++;
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
            if (ByNames.ContainsKey(name)) throw new Exception(string.Format("Column name {0} not found.", name));
            if (ByNames[name].Index < 0 || ByNames[name].Index > ByIndices.Count) throw new Exception(string.Format("Invalid Column Index of {0}", name));
            return ByNames[name].Index;
        }

        /// <summary>
        /// Get the list of PKs
        /// </summary>
        /// <returns></returns>
        public List<IColumnMetadata> GetPKs()
        {
            List<IColumnMetadata> pkList = new List<IColumnMetadata>();

            foreach (var item in ByIndices)
            {
                if ((item.Constraint & EColumnConstraint.PRIMARYKEY) > 0)
                    pkList.Add(item);
            }

            return pkList;
        }

        #endregion Utility

        #region Insert

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

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DynamicColumnHeaders() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

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
