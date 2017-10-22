using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Types
{
    /// <summary>
    /// The class which represents the DateTime datatype of a database
    /// </summary>
    public class DbDateTimeDataType : DbDataType
    {
        /// <summary>
        /// The format of the DateTime as stored in the database
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DbDateTimeDataType() { }

        /// <summary>
        /// Constructor with format
        /// </summary>
        /// <param name="format"></param>
        public DbDateTimeDataType(string format)
        {
            Format = format;
        }
    }
}
