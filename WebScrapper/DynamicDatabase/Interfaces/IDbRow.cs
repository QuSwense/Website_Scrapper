using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DynamicDatabase.Interfaces
{
    /// <summary>
    /// A generic interface for classes of type <see cref="DynamicRow"/>
    /// </summary>
    public interface IDbRow : IDisposable
    {
        /// <summary>
        /// Refers to the parent table
        /// </summary>
        IDbTable Table { get; }

        /// <summary>
        /// The data by column
        /// </summary>
        DynamicColumns Columns { get; }

        /// <summary>
        /// Set the dirty flag for a new insert or update
        /// </summary>
        bool IsDirty { get; set; }

        /// <summary>
        /// Constructor with parent table
        /// </summary>
        /// <param name="table"></param>
        void Initialize(IDbTable table);

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="reader"></param>
        void AddorUpdate(DbDataReader reader);

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        void AddorUpdate(string name, object data);

        /// <summary>
        /// Add a column value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        void AddorUpdate(int index, object data);

        /// <summary>
        /// Update this instance of row from the argument
        /// </summary>
        /// <param name="row"></param>
        void Update(IDbRow row);

        /// <summary>
        /// Get the unique key representation of PK
        /// </summary>
        /// <returns></returns>
        string ToStringByPK();

        /// <summary>
        /// Get the unique key representation of PK
        /// </summary> 
        /// <returns></returns>
        string ToStringByPK(List<string> pkCols);

        /// <summary>
        /// Add data by indexed list
        /// </summary>
        /// <param name="dataList"></param>
        void AddorUpdate(List<string> dataList);

        /// <summary>
        /// Try to get the column data without throwing any exception.
        /// If not found then return null
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        DbDataType TryGetValue(int index);
    }
}
