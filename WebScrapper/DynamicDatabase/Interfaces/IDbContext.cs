using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Interfaces
{
    /// <summary>
    /// The database context interface
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Get the data type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        string GetDataType(Type propertyType);
    }
}
