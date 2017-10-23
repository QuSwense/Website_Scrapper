using DynamicDatabase.Interfaces;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DynamicDatabase
{
    public class DynamicColumns : IDisposable
    {
        /// <summary>
        /// Refers to the parent table
        /// </summary>
        public IDbTable Table { get; protected set; }

        /// <summary>
        /// The data set which contains column data by name
        /// </summary>
        public Dictionary<string, DbDataType> ByNames { get; protected set; }

        /// <summary>
        /// The data set which dcontains column data by index
        /// </summary>
        public List<DbDataType> ByIndices { get; protected set; }

        /// <summary>
        /// An indexer to access data like array using index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (ByIndices == null || ByIndices.Count <= index) throw new IndexOutOfRangeException("Column Index out of range");
                return ByIndices[index];
            }
            set
            {
                if (ByIndices == null || ByIndices.Count <= index) throw new IndexOutOfRangeException("Column Index out of range");
                ByIndices[index].Value = value;
            }
        }

        /// <summary>
        /// An indexer to access data like array using column name
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                if (ByNames == null || !ByNames.ContainsKey(name)) throw new IndexOutOfRangeException("Column name do not exists");
                return ByNames[name];
            }
            set
            {
                if (ByNames == null || !ByNames.ContainsKey(name)) throw new IndexOutOfRangeException("Column name do not exists");
                ByNames[name].Value = value;
            }
        }

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
                dt = DbDataTypeHelper.ParseDataType(data.GetType());
                ByNames.Add(name, dt);
            }

            ByNames[name].Value = data;
        }

        /// <summary>
        /// Add or update a column data in the list dataset.
        /// Thsi will automatically update the dictionary dataset
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void AddorUpdate(int index, object data)
        {
            if (ByIndices == null) ByIndices = new List<DbDataType>();
            DbDataType dt;

            if (ByIndices.Count >= index || ByIndices[index] == null)
            {
                dt = DbDataTypeHelper.ParseDataType(data.GetType());
                ByIndices.Insert(index, dt);
            }
            else dt = ByIndices[index];

            dt.Value = data;
        }

        /// <summary>
        /// Create a unique string identifier for this Coloumn data using only the primary keys
        /// </summary>
        /// <returns></returns>
        public string ToStringPK()
        {
            if (Table == null) throw new Exception("This class is not associated with any Table");

            List<string> pkList = Table.GetPKNames();
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
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
