using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Db.Ctx
{
    /// <summary>
    /// The class represents the row of a table
    /// </summary>
    public class DynamicRow
    {
        /// <summary>
        /// The unique row id of the table
        /// </summary>
        public string RowId { get; protected set; }

        /// <summary>
        /// The row of a table with rowid as key
        /// </summary>
        public Dictionary<string, object> Columns { get; protected set; }

        public DynamicRow() { }
    }
}
