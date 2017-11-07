using DynamicDatabase.Config;
using System;

namespace DynamicDatabase.Types
{
    public static class DbDataTypeHelper
    {
        /// <summary>
        /// Parse data type from the type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public static DbDataType ParseDataType(Type propertyType)
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
        public static DbDataType ParseDataType(ConfigDbColumn colConfig)
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
        public static DbDataType Clone(DbDataType dataType)
        {
            if (dataType is DbCharDataType) return new DbCharDataType();
            else if (dataType is DbDateTimeDataType) return new DbDateTimeDataType();
            else if (dataType is DbDoubleDataType) return new DbDoubleDataType();
            else if (dataType is DbIntDataType) return new DbIntDataType();
            else throw new Exception("Unknwon database dta type");
        }

        public static string GetValue(DbDataType dataType, object value)
        {
            if (value == null) return "NULL";
            if (dataType is DbCharDataType || dataType is DbDateTimeDataType)
            {
                return string.Format("'{0}'", value.ToString());
            }
            else if (dataType is DbDateTimeDataType)
            {
                return string.Format("'{0}'", value.ToString());
            }
            else if (dataType is DbDoubleDataType)
            {
                DbDoubleDataType doubleDt = dataType as DbDoubleDataType;
                string format = null;
                if (doubleDt.CountAfterDecimal >= 0) format = "{0:N" + doubleDt.CountAfterDecimal + "}";
                else format = "{0}";
                return string.Format(format, value.ToString());
            }
            else if (dataType is DbIntDataType)
            {
                return value.ToString();
            }
            else throw new Exception("Unknwon database dta type");
        }
    }
}
