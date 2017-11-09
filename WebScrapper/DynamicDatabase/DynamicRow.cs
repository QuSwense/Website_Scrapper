using DynamicDatabase.Interfaces;
using DynamicDatabase.Types;
using System.Collections.Generic;
using System.Data.Common;

namespace DynamicDatabase
{
    /// <summary>
    /// The class represents the row of a table
    /// </summary>
    public class DynamicRow : IDbRow
    {
        #region Properties

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
        /// Set the dirty flag for a new insert or update
        /// </summary>
        public bool IsDirty { get; set; }

        #endregion Properties

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
        /// Add a column value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public virtual void AddorUpdate(string name, object data)
        {
            if (Columns == null)
            {
                Columns = new DynamicColumns();
                Columns.Initialize(this);
            }
            Columns.AddorUpdate(name, data);
            IsDirty = true;
        }

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public virtual void AddorUpdate(int index, object data)
        {
            if (Columns == null)
            {
                Columns = new DynamicColumns();
                Columns.Initialize(this);
            }
            Columns.AddorUpdate(index, data);
            IsDirty = true;
        }

        /// <summary>
        /// Add data by indexed list
        /// </summary>
        /// <param name="dataList"></param>
        public void AddorUpdate(List<string> dataList)
        {
            if (Columns == null)
            {
                Columns = new DynamicColumns();
                Columns.Initialize(this);
            }
            for (int i = 0; i < dataList.Count; i++)
            {
                Columns[i] = DbDataTypeHelper.Clone(Table.Headers[i].DataType);
                Columns[i].Value = dataList[i];
            }
            IsDirty = true;
        }

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="reader"></param>
        public void AddorUpdate(DbDataReader reader)
        {
            if (Columns == null)
            {
                Columns = new DynamicColumns();
                Columns.Initialize(this);
            }

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Columns[i] = DbDataTypeHelper.ParseDataType(reader.GetFieldType(i));
                Columns[i].Value = reader.GetValue(i);
            }
            IsDirty = true;
        }

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
        public string ToStringByPK()
        {
            if (RowId != null)
                return string.Join(",", RowId, Columns.ToStringPK());
            else
                return Columns.ToStringPK();
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
            if (RowId != null)
                return string.Join(",", RowId, Columns.ToString());
            else
                return Columns.ToString();
        }

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
