using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using DynamicDatabase.Types;

namespace DynamicDatabase
{
    public class DynamicRows : IDisposable
    {
        /// <summary>
        /// Refers to the parent table
        /// </summary>
        public IDbTable Table { get; protected set; }

        /// <summary>
        /// The rows in the table by index
        /// </summary>
        public List<IDbRow> ByIndices { get; protected set; }

        /// <summary>
        /// The rows in the table by unique key names
        /// This data structure will be initialized only for specific use
        /// </summary>
        public Dictionary<string, IDbRow> ByNames { get; protected set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DynamicRows() { }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="table"></param>
        public DynamicRows(IDbTable table)
        {
            Table = table;
            ByIndices = new List<IDbRow>();
        }

        /// <summary>
        /// An indexer to access data like array using index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IDbRow this[int index]
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
        public IDbRow this[string name]
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

        /// <summary>
        /// Load a row from the current reader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="cols"></param>
        public void Load(DbDataReader reader, string[] cols)
        {
            IDbRow row = Table.DbContext.DbFactory.Create<IDbRow>();
            row.AddorUpdate(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fnCriteria"></param>
        /// <returns></returns>
        public IDbRow FindByPK(string uniqueKeyString)
        {
            IDbRow result = null;
            ByNames.TryGetValue(uniqueKeyString, out result);
            return result;
        }

        /// <summary>
        /// Load data in memory by Rowid
        /// </summary>
        /// <param name="reader"></param>
        public virtual void AddOrUpdate(DbDataReader reader)
        {
            var row = Table.DbContext.DbFactory.Create<IDbRow>();
            row.Initialize(Table);

            List<DbDataType> pks = new List<DbDataType>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Columns[i] = DbDataTypeHelper.ParseDataType(reader.GetFieldType(i));
                row.Columns[i].Value = reader.GetValue(i);

                if ((Table.Headers[i].Constraint & EColumnConstraint.PRIMARYKEY) > 0)
                    pks.Add(row.Columns[i]);
            }

            if (ByIndices == null) ByIndices = new List<IDbRow>();
            ByIndices.Add(row);

            if (ByNames == null) ByNames = new Dictionary<string, IDbRow>();
            string pkString = DynamicDbHelper.GetPrimaryKeyString(pks);

            if (ByNames.ContainsKey(pkString)) throw new Exception("Duplicate data insertion not allowed by Primary key string : " + pkString);
            ByNames.Add(pkString, row);
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
