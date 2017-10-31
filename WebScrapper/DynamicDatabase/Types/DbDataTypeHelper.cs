using DynamicDatabase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
