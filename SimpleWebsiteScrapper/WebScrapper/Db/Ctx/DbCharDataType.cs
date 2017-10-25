using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Db.Ctx
{
    /// <summary>
    /// Char data type class. This class represents any text data type.
    /// </summary>
    public class DbCharDataType : DbDataType
    {
        /// <summary>
        /// The total number characters
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Default
        /// </summary>
        public DbCharDataType() { }

        /// <summary>
        /// Constructor with count
        /// </summary>
        /// <param name="count"></param>
        public DbCharDataType(int count)
        {
            Count = count;
        }
    }
}
