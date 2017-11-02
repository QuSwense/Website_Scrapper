using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Types
{
    public class DataTypeContext : IDataTypeContext
    {
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

        //public static DbDataType Deduce(object value, Type type)
        //{
        //    DbDataType dt = null;

        //    if(type is byte)
        //        dt = new DbIntDataType()
        //}
    }
}
