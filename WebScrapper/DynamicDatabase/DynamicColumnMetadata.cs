using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Common;
using DynamicDatabase.Types;
using DynamicDatabase.Config;
using DynamicDatabase.Meta;
using WebCommon.Extn;
using DynamicDatabase.Interfaces;

namespace DynamicDatabase
{
    /// <summary>
    /// Class represents metdata of a column
    /// </summary>
    public class DynamicColumnMetadata : IDisposable, IColumnMetadata
    {
        /// <summary>
        /// Refers to the parent table
        /// </summary>
        public IDbTable Table { get; protected set; }

        /// <summary>
        /// The name
        /// </summary>
        public string ColumnName { get; protected set; }

        /// <summary>
        /// Default contraint
        /// </summary>
        public string Default { get; protected set; }

        /// <summary>
        /// The Database data type
        /// </summary>
        public DbDataType DataType { get; protected set; }

        /// <summary>
        /// The constraints
        /// </summary>
        public EColumnConstraint Constraint { get; protected set; }

        /// <summary>
        /// Get the index of the column
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// Parse the column configuration object
        /// </summary>
        /// <param name="colname"></param>
        /// <param name="colConfig"></param>
        public void Parse(string colname, ConfigDbColumn colConfig)
        {
            ColumnName = colname;
            DataType = DbDataTypeHelper.ParseDataType(colConfig);
            Constraint = ParseConstraint(colConfig);
        }

        /// <summary>
        /// Parse the database reader.
        /// It is assumed that all Database Meta column info query will follow the same format
        /// Column 2 - name
        /// Column 3 - Data Type
        /// Column Rest - Constraints
        /// </summary>
        /// <param name="reader"></param>
        public virtual void Parse(DbDataReader reader)
        {
            ColumnName = reader.GetString(1);
            DataType = Table.DbContext.DbDataType.ParseDataType(reader.GetString(2));
            Constraint = ParseConstraint(reader);
        }

        /// <summary>
        /// This method is used to parse the output of a database metadata query
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual EColumnConstraint ParseConstraint(DbDataReader reader)
        {
            return EColumnConstraint.NONE;
        }

        /// <summary>
        /// Parse data from the property attributes
        /// </summary>
        /// <param name="prop"></param>
        public void Parse(PropertyInfo prop)
        {
            DDColumnAttribute colAttr = prop.GetCustomAttribute<DDColumnAttribute>();
            DDNotNullAttribute notNullAttr = prop.GetCustomAttribute<DDNotNullAttribute>();
            DDPrimaryKeyAttribute pkAttr = prop.GetCustomAttribute<DDPrimaryKeyAttribute>();
            DDUniqueAttribute uniqueAttr = prop.GetCustomAttribute<DDUniqueAttribute>();

            ColumnName = colAttr.Name;
            DataType = DbDataTypeHelper.ParseDataType(prop.PropertyType);

            if (notNullAttr != null) Constraint |= EColumnConstraint.UNQIUE;
            if (pkAttr != null) Constraint |= EColumnConstraint.PRIMARYKEY;
            if (uniqueAttr != null) Constraint |= EColumnConstraint.UNQIUE;
        }

        /// <summary>
        /// Parse the constriant data of column configuration object
        /// </summary>
        /// <param name="colConfig"></param>
        /// <returns></returns>
        private EColumnConstraint ParseConstraint(ConfigDbColumn colConfig)
        {
            EColumnConstraint constraint = EColumnConstraint.NONE;

            if (colConfig.IsPrimaryKey) constraint |= EColumnConstraint.PRIMARYKEY;
            if (colConfig.Unique) constraint |= EColumnConstraint.UNQIUE;

            return constraint;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DataType = null;
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
