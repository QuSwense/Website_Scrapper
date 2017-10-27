using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// The unique row id of the table
        /// </summary>
        string RowId { get; }

        /// <summary>
        /// The data by column
        /// </summary>
        DynamicColumns Columns { get; }

        /// <summary>
        /// Constructor with parent table
        /// </summary>
        /// <param name="table"></param>
        void Initialize(IDbTable table);

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
        /// Get the unique key representation of PK
        /// </summary>
        /// <returns></returns>
        string ToStringByPK();

        /// <summary>
        /// Get the unique key representation of PK
        /// </summary> 
        /// <returns></returns>
        string ToStringByPK(List<string> pkCols);
    }
}
