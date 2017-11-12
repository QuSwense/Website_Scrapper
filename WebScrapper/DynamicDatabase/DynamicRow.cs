using DynamicDatabase.Interfaces;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DynamicDatabase
{
    /// <summary>
    /// The class represents a single row of a table
    /// </summary>
    public class DynamicRow : IDbRow
    {
        #region Properties

        /// <summary>
        /// Refers to the parent table
        /// </summary>
        public IDbTable Table { get; protected set; }
        
        /// <summary>
        /// The data by column
        /// </summary>
        public DynamicColumns Columns { get; protected set; }

        /// <summary>
        /// Set the dirty flag for a new insert or update
        /// </summary>
        public bool IsDirty { get; set; }

        #endregion Properties

        #region Properties Helper

        public IDataTypeContext DbDataType
        {
            get { return Table.DbContext.DbDataType; }
        }

        #endregion Properties Helper

        #region Constructor

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
            IsDirty = false;
        }

        #endregion Constructor

        #region Insert

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="fAddAction"></param>
        protected void AddHelper(Action fAddAction)
        {
            if (Columns == null) Columns = new DynamicColumns(this);
            fAddAction();
            IsDirty = true;
        }

        /// <summary>
        /// Add a new row
        /// </summary>
        /// <param name="colMetadata"></param>
        /// <param name="reader"></param>
        public virtual void Add(IColumnMetadata colMetadata, DbDataReader reader)
            => AddHelper(() => Columns.Add(colMetadata, reader));

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public virtual void AddOrUpdate(string name, object data)
            => AddHelper(() => Columns.AddOrUpdate(name, data));

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public virtual void AddOrUpdate(int index, object data)
            => AddHelper(() => Columns.AddOrUpdate(index, data));

        /// <summary>
        /// Add data by indexed list
        /// </summary>
        /// <param name="dataList"></param>
        public void AddOrUpdate(IEnumerable<string> colDataArray)
            => AddHelper(() =>
            {
                List<string> dataList = colDataArray.ToList();
                for (int i = 0; i < dataList.Count(); i++) Columns.AddOrUpdate(i, dataList[i]);
            });

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="reader"></param>
        public void AddOrUpdate(DbDataReader reader)
            => AddHelper(() =>
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Columns.AddOrUpdate(i, reader.GetValue(i));
            });

        /// <summary>
        /// Update this instance of row from the argument
        /// </summary>
        /// <param name="row"></param>
        public void Update(IDbRow row)
        {
            Columns.Update(row.Columns);
            IsDirty = true;
        }

        #endregion Insert

        #region Utility

        /// <summary>
        /// Get the unique key representation of PK
        /// </summary>
        /// <returns></returns>
        public string ToStringByPK() => Columns.ToStringPK();

        /// <summary>
        /// Get the unique key representation of unique keys
        /// </summary>
        /// <returns></returns>
        public string ToStringByUK() => Columns.ToStringUK();

        /// <summary>
        /// Get the unique key representation of PK
        /// </summary> 
        /// <returns></returns>
        public virtual string ToStringByPK(List<string> pkCols)
            => Columns.ToStringPK(pkCols);

        /// <summary>
        /// Get all keys by PK
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Columns.ToString();

        /// <summary>
        /// Try to get the column data without throwing any exception.
        /// If not found then return null
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DbDataType TryGetValue(int index)
        {
            if (Columns == null || Columns.ByIndices.Count <= index) return null;
            return Columns[index];
        }

        #endregion Utility

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) Columns.Dispose();

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
