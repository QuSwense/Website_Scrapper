using DynamicDatabase.Config;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace DynamicDatabase.Interfaces
{
    public interface IColumnHeaders :
        IEnumerator<IColumnMetadata>,
        IEnumerable<IColumnMetadata>, IDisposable
    {
        #region Properties

        /// <summary>
        /// Refers to the parent table
        /// </summary>
        IDbTable Table { get; }

        /// <summary>
        /// The list of column headers by name
        /// </summary>
        Dictionary<string, IColumnMetadata> ByNames { get; }

        /// <summary>
        /// The list of column headers by index
        /// </summary>
        List<IColumnMetadata> ByIndices { get; }

        #endregion Properties

        #region Indexer

        /// <summary>
        /// An indexer to access data like array using index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IColumnMetadata this[int index] { get; set; }

        /// <summary>
        /// An indexer to access data like array using column name
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IColumnMetadata this[string name] { get; set; }

        #endregion Indexer

        #region Constructor

        /// <summary>
        /// Initialize the column headers using config
        /// </summary>
        /// <param name="configCols"></param>
        void Initialize(IDbTable tableParent, Dictionary<string, ConfigDbColumn> configCols);

        /// <summary>
        /// Initialize from the metdata reader
        /// </summary>
        /// <param name="reader"></param>
        void Initialize(IDbTable tableParent, DbDataReader reader);

        #endregion Constructor

        #region Utility

        /// <summary>
        /// Get the index of the column
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetColumnIndex(string name);

        /// <summary>
        /// Get the list of PKs
        /// </summary>
        /// <returns></returns>
        List<IColumnMetadata> GetPKs();

        #endregion Utility
    }
}
