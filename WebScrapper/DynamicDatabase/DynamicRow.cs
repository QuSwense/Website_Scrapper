using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase
{
    /// <summary>
    /// The class represents the row of a table
    /// </summary>
    public class DynamicRow : IDbRow
    {
        /// <summary>
        /// Refers to the parent table
        /// </summary>
        public IDbTable Table { get; protected set; }

        /// <summary>
        /// The unique row id of the table
        /// </summary>
        public string RowId { get; protected set; }

        /// <summary>
        /// The data by column
        /// </summary>
        public DynamicColumns Columns { get; protected set; }

        /// <summary>
        /// Default constructor not used
        /// </summary>
        public DynamicRow() { }

        /// <summary>
        /// Constructor with parent table
        /// </summary>
        /// <param name="table"></param>
        public void Initialize(IDbTable table)
        {
            Table = table;
        }

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public virtual void AddorUpdate(string name, object data)
        {
            if (Columns == null) Columns = new DynamicColumns();
            Columns.AddorUpdate(name, data);
        }

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public virtual void AddorUpdate(int index, object data)
        {
            if (Columns == null) Columns = new DynamicColumns();
            Columns.AddorUpdate(index, data);
        }

        /// <summary>
        /// Add data by indexed list
        /// </summary>
        /// <param name="dataList"></param>
        public void AddorUpdate(List<string> dataList)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                Columns[i] = dataList[i];
            }
        }

        /// <summary>
        /// Get the unique key representation of PK
        /// </summary>
        /// <returns></returns>
        public string ToStringByPK()
        {
            return string.Join(",", RowId, Columns.ToStringPK());
        }

        /// <summary>
        /// Get the unique key representation of PK
        /// </summary> 
        /// <returns></returns>
        public virtual string ToStringByPK(List<string> pkCols)
        {
            return Columns.ToStringPK(pkCols);
        }

        /// <summary>
        /// Get all keys by PK
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(",", RowId, Columns.ToString());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Columns.Dispose();
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
