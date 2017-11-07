﻿using DynamicDatabase.Config;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace DynamicDatabase.Interfaces
{
    public interface IColumnHeaders :
        IEnumerator<IColumnMetadata>,
        IEnumerable<IColumnMetadata>
    {
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

        /// <summary>
        /// Initialize the column headers using config
        /// </summary>
        /// <param name="configCols"></param>
        void Initialize(Dictionary<string, ConfigDbColumn> configCols);

        /// <summary>
        /// Initialize Headers using Column metdata models
        /// </summary>
        /// <param name="classProperties"></param>
        void Initialize(PropertyInfo[] classProperties);

        /// <summary>
        /// Get the index of the column
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetColumnIndex(string name);

        /// <summary>
        /// Initialize from the metdata reader
        /// </summary>
        /// <param name="reader"></param>
        void Initialize(DbDataReader reader);

        /// <summary>
        /// Get the list of PKs
        /// </summary>
        /// <returns></returns>
        List<IColumnMetadata> GetPKs();
    }
}
