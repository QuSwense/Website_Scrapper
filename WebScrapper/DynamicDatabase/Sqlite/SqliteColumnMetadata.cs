using DynamicDatabase;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebScrapper.Db.Ctx.sqlite
{
    /// <summary>
    /// The class which represents metdata about a Sqlite table column
    /// </summary>
    public class SqliteColumnMetadata : DynamicColumnMetadata
    {
        /// <summary>
        /// Parses a sqlite metdata information collected from the PRAGMA query
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override EColumnConstraint ParseConstraint(DbDataReader reader)
        {
            if (reader.GetInt32(3) == 1) Constraint |= EColumnConstraint.NOTNULL;
            if (!string.IsNullOrEmpty(reader.GetString(4)))
            {
                Constraint |= EColumnConstraint.DEFAULT;
                Default = reader.GetString(4);
            }
            if (reader.GetInt32(5) == 1) Constraint |= EColumnConstraint.PRIMARYKEY;

            return Constraint;
        }
    }
}
