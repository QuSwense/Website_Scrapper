using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using DynamicDatabase.Config;
using System.Collections;
using DynamicDatabase.Interfaces;

namespace DynamicDatabase
{
    public class DynamicColumnHeaders : IColumnHeaders
    {
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

        /// <summary>
        /// Initialize the column headers using config
        /// </summary>
        /// <param name="configCols"></param>
        public void Initialize(Dictionary<string, ConfigDbColumn> configCols)
        {
            int index = 0;
            foreach (var item in configCols)
            {
                IColumnMetadata colMetadata = DynamicDbFactory.Create<IColumnMetadata>();
                colMetadata.Parse(item.Key, item.Value);
                AddHeader(index, item.Key, colMetadata);
                index++;
            }
        }

        /// <summary>
        /// Initialize Headers using Column metdata models
        /// </summary>
        /// <param name="classProperties"></param>
        public void Initialize(PropertyInfo[] classProperties)
        {
            int index = 0;
            foreach (PropertyInfo prop in classProperties)
            {
                IColumnMetadata colMetadata = DynamicDbFactory.Create<IColumnMetadata>();
                colMetadata.Parse(prop);
                AddHeader(index, colMetadata.ColumnName, colMetadata);
                index++;
            }
        }

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
        /// Initialize from the metdata reader
        /// </summary>
        /// <param name="reader"></param>
        public void Initialize(DbDataReader reader)
        {
            int index = 0;
            while (reader.Read())
            {
                IColumnMetadata colMetadata = Table.DbContext.DbFactory.Create<IColumnMetadata>();
                colMetadata.Parse(reader);
                AddHeader(index, colMetadata.ColumnName, colMetadata);
                index++;
            }
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

        /// <summary>
        /// Get the list of PKs
        /// </summary>
        /// <returns></returns>
        public List<IColumnMetadata> GetPKs()
        {
            List<IColumnMetadata> pkList = new List<IColumnMetadata>();

            foreach(var item in ByIndices)
            {
                if ((item.Constraint & EColumnConstraint.PRIMARYKEY) > 0)
                    pkList.Add(item);
            }

            return pkList;
        }

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
        
        public void Dispose()
        {
            
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
    }
}
