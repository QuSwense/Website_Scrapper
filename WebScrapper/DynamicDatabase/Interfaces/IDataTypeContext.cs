using DynamicDatabase.Config;
using DynamicDatabase.Types;
using System;

namespace DynamicDatabase.Interfaces
{
    public interface IDataTypeContext
    {
        /// <summary>
        /// Get the sqlitre data tyupe from the <see cref="Type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetDataType(Type type);

        /// <summary>
        /// Parses the data type from a string and returns the generic <see cref="DbDataType"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        DbDataType ParseDataType(string typeName);

        /// <summary>
        /// Parse data type from the type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        DbDataType ParseDataType(Type propertyType);

        /// <summary>
        /// Parse the data type from column configuration object
        /// </summary>
        /// <param name="colConfig"></param>
        /// <returns></returns>
        DbDataType ParseDataType(ConfigDbColumn colConfig);

        /// <summary>
        /// Clone a Database data type
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        DbDataType Clone(DbDataType dataType);

        /// <summary>
        /// Get the value from the data type
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetValue(DbDataType dataType, object value);

        /// <summary>
        /// Get the value from the data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string GetValue(DbDataType data);
    }
}
