using DynamicDatabase.Config;
using DynamicDatabase.Types;
using System;
using System.Data.Common;
using System.Reflection;

namespace DynamicDatabase.Interfaces
{
    /// <summary>
    /// The interface for Column metadata class
    /// </summary>
    public interface IColumnMetadata : IDisposable
    {
        #region Properties

        /// <summary>
        /// Refers to the parent table
        /// </summary>
        IDbTable Table { get; }

        /// <summary>
        /// The name
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Default contraint
        /// </summary>
        object Default { get; }

        /// <summary>
        /// The Database data type
        /// </summary>
        DbDataType DataType { get; }

        /// <summary>
        /// The constraints
        /// </summary>
        EColumnConstraint Constraint { get; }

        /// <summary>
        /// A boolean value to check if this column is primary key
        /// </summary>
        bool IsPK { get; }

        /// <summary>
        /// A boolean value to check if this column is primary key
        /// </summary>
        bool IsNotNull { get; }

        /// <summary>
        /// A boolean value to check if this column is primary key
        /// </summary>
        bool IsUnique { get; }

        /// <summary>
        /// Get the index of the column
        /// </summary>
        int Index { get; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Use this initializer
        /// </summary>
        /// <param name="dynTable"></param>
        void Initialize(IDbTable dynTable);

        /// <summary>
        /// Update the current column metdata using the model object
        /// </summary>
        /// <param name="colLoadObj"></param>
        void Merge(ColumnLoadDataModel colLoadObj);

        #endregion Constructor

        #region Load

        /// <summary>
        /// Parse the column configuration object
        /// </summary>
        /// <param name="colname"></param>
        /// <param name="colConfig"></param>
        void Parse(string colname, ConfigDbColumn colConfig);

        /// <summary>
        /// Parse the database reader.
        /// It is assumed that all Database Meta column info query will follow the same format
        /// Column 2 - name
        /// Column 3 - Data Type
        /// Column Rest - Constraints
        /// </summary>
        /// <param name="reader"></param>
        void Parse(DbDataReader reader);
        
        /// <summary>
        /// Parse data from the property attributes
        /// </summary>
        /// <param name="prop"></param>
        void Parse(PropertyInfo prop);

        #endregion Load
    }
}
