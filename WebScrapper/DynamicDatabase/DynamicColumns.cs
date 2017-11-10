using DynamicDatabase.Interfaces;
using DynamicDatabase.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DynamicDatabase
{
    /// <summary>
    /// Represents the data stored in a single row in terms of column information
    /// </summary>
    public class DynamicColumns : IDisposable,
        IEnumerator<DbDataType>,
        IEnumerable<DbDataType>
    {
        #region Properties

        /// <summary>
        /// Refers to the parent row
        /// </summary>
        public IDbRow Row { get; protected set; }

        /// <summary>
        /// The data set which contains column data by name
        /// </summary>
        public Dictionary<string, DbDataType> ByNames { get; protected set; }

        /// <summary>
        /// The data set which contains column data by index
        /// </summary>
        public List<DbDataType> ByIndices { get; protected set; }

        /// <summary>
        /// A internal property to simplify the access to the DataType Context object
        /// </summary>
        protected IDataTypeContext DbDataType
        {
            get
            {
                return Row.Table.DbContext.DbDataType;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamicColumns() { }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="rowParent"></param>
        public DynamicColumns(IDbRow rowParent)
        {
            Row = rowParent;
            ByIndices = new List<DbDataType>();
            ByNames = new Dictionary<string, DbDataType>();
        }

        #endregion Constructor

        #region Indexer

        /// <summary>
        /// An indexer to access data like array using index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DbDataType this[int index]
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
        public DbDataType this[string name]
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

        #region Insert

        /// <summary>
        /// Add or update a column data in the Dictionary data set.
        /// That will automatically update the reference in the List dataset
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void AddorUpdate(string name, object data)
        {
            DbDataType dt;

            // Check if Dictionary contains the data if not add
            if (!ByNames.ContainsKey(name))
            {
                dt = DbDataType.ParseDataType(data.GetType());
                ByNames.Add(name, dt);
                ByIndices.Add(dt);
            }
            else dt = ByNames[name];

            dt.Value = data;
        }

        /// <summary>
        /// Add or update a column data in the list dataset.
        /// Thsi will automatically update the dictionary dataset
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void AddorUpdate(int index, object data)
        {
            DbDataType dt;

            if (ByIndices.Count >= index || ByIndices[index] == null)
            {
                dt = DbDataType.ParseDataType(data.GetType());
                ByIndices.Insert(index, dt);
                ByNames.Add(Row.Table.Headers[index].ColumnName, dt);
            }
            else dt = ByIndices[index];

            dt.Value = data;
        }

        /// <summary>
        /// Update the columns of this instance from the passed instance
        /// </summary>
        /// <param name="columns"></param>
        public void Update(DynamicColumns columns)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                this[i] = columns.ByIndices[i];
            }
        }

        #endregion Insert

        #region Utility

        /// <summary>
        /// Create a unique string identifier for this Coloumn data using only the primary keys
        /// </summary>
        /// <returns></returns>
        public string ToStringPK()
        {
            if (Row == null) throw new Exception("This class is not associated with any row");

            List<string> pkList = Row.Table.GetPKNames();
            string pkString = "";

            if(pkList != null && pkList.Count > 0 && ByNames != null)
            {
                List<string> pkvalues = ByNames.Where(p => pkList.Contains(p.Key)).Select(p => p.Value.Value.ToString()).ToList();
                if (pkvalues.Count <= 0) throw new Exception("Internal: No Primary Key Values found");
                if (pkvalues.Count != pkList.Count) throw new Exception("Internal: Mismatch in the count of Primary keys");
                pkString = string.Join(",", pkvalues);
            }

            return pkString;
        }

        /// <summary>
        /// Resolve to String data
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (ByIndices == null || ByIndices.Count <= 0) throw new Exception("Column Data not initialized");
            return string.Join(",", ByIndices.Select(p => p.Value.ToString()).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkCols"></param>
        /// <returns></returns>
        internal string ToStringPK(List<string> pkCols)
        {
            return string.Join(",",ByNames.Where(c => pkCols.Contains(c.Key))
                .Select(p => p.Value.Value).ToList());
        }

        #endregion Utility

        #region IEnumerator
        private int _position = -1;

        public DbDataType Current
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

        IEnumerator<DbDataType> GetEnumerator()
        {
            return ByIndices.GetEnumerator();
        }

        IEnumerator<DbDataType> IEnumerable<DbDataType>.GetEnumerator()
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
                    ByNames.Clear();
                    ByIndices.Clear();
                }
                
                disposedValue = true;
            }
        }
        
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
