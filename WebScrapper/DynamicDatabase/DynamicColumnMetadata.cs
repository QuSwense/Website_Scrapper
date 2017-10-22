using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Common;
using DynamicDatabase.Types;
using DynamicDatabase.Config;
using DynamicDatabase.Meta;

namespace DynamicDatabase
{
    /// <summary>
    /// Class represents metdata of a column
    /// </summary>
    public class DynamicColumnMetadata
    {
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
        /// Parse the column configuration object
        /// </summary>
        /// <param name="colname"></param>
        /// <param name="colConfig"></param>
        public void Parse(string colname, ColumnDbConfig colConfig)
        {
            ColumnName = colname;
            DataType = ParseDataType(colConfig);
            Constraint = ParseConstraint(colConfig);
        }

        /// <summary>
        /// Parse the database reader
        /// </summary>
        /// <param name="reader"></param>
        public void Parse(DbDataReader reader)
        {
            ColumnName = reader.GetString(1);
            DataType = ParseDataType(reader.GetString(2));
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
        internal void Parse(PropertyInfo prop)
        {
            DDColumnAttribute colAttr = prop.GetCustomAttribute<DDColumnAttribute>();
            DDNotNullAttribute notNullAttr = prop.GetCustomAttribute<DDNotNullAttribute>();
            DDPrimaryKeyAttribute pkAttr = prop.GetCustomAttribute<DDPrimaryKeyAttribute>();
            DDUniqueAttribute uniqueAttr = prop.GetCustomAttribute<DDUniqueAttribute>();

            ColumnName = colAttr.Name;
            DataType = ParseDataType(prop.PropertyType);

            if (notNullAttr != null) Constraint |= EColumnConstraint.UNQIUE;
            if (pkAttr != null) Constraint |= EColumnConstraint.PRIMARYKEY;
            if (uniqueAttr != null) Constraint |= EColumnConstraint.UNQIUE;
        }

        /// <summary>
        /// Parse the constriant data of column configuration object
        /// </summary>
        /// <param name="colConfig"></param>
        /// <returns></returns>
        private EColumnConstraint ParseConstraint(ColumnDbConfig colConfig)
        {
            EColumnConstraint constraint = EColumnConstraint.NONE;

            if (colConfig.IsPrimaryKey) constraint |= EColumnConstraint.PRIMARYKEY;
            if (colConfig.Unique) constraint |= EColumnConstraint.UNQIUE;

            return constraint;
        }

        /// <summary>
        /// Parse the data type from column configuration object
        /// </summary>
        /// <param name="colConfig"></param>
        /// <returns></returns>
        private DbDataType ParseDataType(ColumnDbConfig colConfig)
        {
            switch (colConfig.DataType)
            {
                case EDataTypeDbConfig.BOOLEAN:
                    return new DbIntDataType(1);
                case EDataTypeDbConfig.DATETIME:
                    return new DbDateTimeDataType();
                case EDataTypeDbConfig.DECIMAL:
                    return new DbDoubleDataType(colConfig.Size, colConfig.Precision);
                case EDataTypeDbConfig.ENUM:
                case EDataTypeDbConfig.NUMBER:
                    return new DbIntDataType(colConfig.Size > 0 ? colConfig.Size : 4);
                case EDataTypeDbConfig.STRING:
                    return new DbCharDataType(colConfig.Size > 0 ? colConfig.Size : 1);
                default:
                    return new DbCharDataType(colConfig.Size > 0 ? colConfig.Size : 200);
            }
        }

        /// <summary>
        /// Parse data type from the string typename
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual DbDataType ParseDataType(string typeName) { return null; }

        /// <summary>
        /// Parse data type from the type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private DbDataType ParseDataType(Type propertyType)
        {
            if (propertyType == typeof(bool))
                return new DbIntDataType(1);
            else if (propertyType == typeof(DateTime))
                return new DbDateTimeDataType();
            else if (propertyType == typeof(double) || propertyType == typeof(decimal))
                return new DbDoubleDataType();
            else if (propertyType == typeof(Enum) || propertyType == typeof(int) || propertyType == typeof(short))
                return new DbIntDataType();
            else
                return new DbCharDataType();
        }
    }
}
