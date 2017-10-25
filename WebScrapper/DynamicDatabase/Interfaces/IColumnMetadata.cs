using DynamicDatabase.Config;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicDatabase.Interfaces
{
    /// <summary>
    /// The interface for Column metadata class
    /// </summary>
    public interface IColumnMetadata : IDisposable
    {
        /// <summary>
        /// The name
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Default contraint
        /// </summary>
        string Default { get; }

        /// <summary>
        /// The Database data type
        /// </summary>
        DbDataType DataType { get; }

        /// <summary>
        /// The constraints
        /// </summary>
        EColumnConstraint Constraint { get; }

        /// <summary>
        /// Get the index of the column
        /// </summary>
        int Index { get; }

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
    }
}
