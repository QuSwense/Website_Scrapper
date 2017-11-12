using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Config
{
    /// <summary>
    /// This model is used by the Database context to load a table in memory with partial information
    /// </summary>
    public class ColumnLoadDataModel
    {
        /// <summary>
        /// The column name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sets if a key is unique
        /// </summary>
        public bool IsUnique { get; set; }
    }
}
