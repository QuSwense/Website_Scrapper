using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using System;

namespace DynamicDatabase.Types
{
    /// <summary>
    /// The Generic class for any data type activity
    /// </summary>
    public class DataTypeContext : IDataTypeContext
    {
        /// <summary>
        /// Get a new data type instance
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public DbDataType New(DbDataType dataType)
        {
            if (dataType is DbCharDataType) return new DbCharDataType();
            if (dataType is DbDateTimeDataType) return new DbDateTimeDataType();
            if (dataType is DbDoubleDataType) return new DbDoubleDataType();
            if (dataType is DbIntDataType) return new DbIntDataType();
            return new DbCharDataType();
        }

        /// <summary>
        /// Get the sqlitre data tyupe from the <see cref="Type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual string GetDataType(Type type)
        {
            return "TEXT";
        }

        /// <summary>
        /// Parses the data type from a string and returns the generic <see cref="DbDataType"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public virtual DbDataType ParseDataType(string typeName)
        {
            return new DbCharDataType();
        }

        /// <summary>
        /// Parse data type from the type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public DbDataType ParseDataType(Type propertyType)
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

        /// <summary>
        /// Parse the data type from column configuration object
        /// </summary>
        /// <param name="colConfig"></param>
        /// <returns></returns>
        public DbDataType ParseDataType(ConfigDbColumn colConfig)
        {
            switch (colConfig.DataType)
            {
                case EConfigDbDataType.BOOLEAN:
                    return new DbIntDataType(1);
                case EConfigDbDataType.DATETIME:
                    return new DbDateTimeDataType();
                case EConfigDbDataType.DECIMAL:
                    return new DbDoubleDataType(colConfig.Size, colConfig.Precision);
                case EConfigDbDataType.ENUM:
                case EConfigDbDataType.NUMBER:
                    return new DbIntDataType(colConfig.Size > 0 ? colConfig.Size : 4);
                case EConfigDbDataType.STRING:
                    return new DbCharDataType(colConfig.Size > 0 ? colConfig.Size : 1);
                default:
                    return new DbCharDataType(colConfig.Size > 0 ? colConfig.Size : 200);
            }
        }

        /// <summary>
        /// Clone a Database data type
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public DbDataType Clone(DbDataType dataType)
        {
            if (dataType is DbCharDataType) return new DbCharDataType();
            else if (dataType is DbDateTimeDataType) return new DbDateTimeDataType();
            else if (dataType is DbDoubleDataType) return new DbDoubleDataType();
            else if (dataType is DbIntDataType) return new DbIntDataType();
            else throw new Exception("Unknwon database dta type");
        }

        /// <summary>
        /// Get the value from the data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetValue(DbDataType data, Func<string, string> fnNormalizeData)
        {
            if (data == null) return "NULL";
            string dataObj = data.Value.ToString();
            dataObj = fnNormalizeData(dataObj);
            if (data is DbCharDataType || data is DbDateTimeDataType)
                dataObj = string.Format("'{0}'", dataObj);
            else if (data is DbDateTimeDataType)
                dataObj = string.Format("'{0}'", dataObj);
            else if (data is DbDoubleDataType)
            {
                DbDoubleDataType doubleDt = data as DbDoubleDataType;
                string format = null;
                if (doubleDt.CountAfterDecimal >= 0) format = "{0:N" + doubleDt.CountAfterDecimal + "}";
                else format = "{0}";
                dataObj = string.Format(format, dataObj);
            }
            else if (data is DbIntDataType)
            {
                dataObj = data.Value.ToString();
            }
            else throw new Exception("Unknwon database dta type");

            return dataObj;
        }
    }
}
