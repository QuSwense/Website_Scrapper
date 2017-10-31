using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase
{
    /// <summary>
    /// A helper class which contains some generic methods to be used everywhere in the Dynamic Db library
    /// equally.
    /// </summary>
    public class DynamicDbHelper
    {
        /// <summary>
        /// Get the string representation of a set of unique keys in database
        /// </summary>
        /// <param name="uKeys"></param>
        /// <returns></returns>
        public static string GetPrimaryKeyString(IEnumerable<DbDataType> uKeys)
            => string.Join(",", uKeys);
    }
}
