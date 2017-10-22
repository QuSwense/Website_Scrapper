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
        /// Parses the data type from a string and returns the generic <see cref="DbDataType"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected override DbDataType ParseDataType(string typeName)
        {
            string typeNameUpper = typeName.ToUpper();

            if (typeNameUpper == "TEXT")
            {
                return new DbCharDataType();
            }
            else if (typeNameUpper == "REAL")
            {
                return new DbDoubleDataType();
            }
            else if (typeNameUpper == "INTEGER")
            {
                return new DbIntDataType();
            }
            else if (typeNameUpper.Contains("VARCHAR"))
            {
                Match match = Regex.Match(typeNameUpper, @"VARCHAR\((\d+)\)");
                int count = 1;

                if (match.Success)
                    count = Convert.ToInt32(match.Groups[0]);
                return new DbCharDataType(count);
            }
            else if (typeNameUpper.Contains("DECIMAL"))
            {
                Match match = Regex.Match(typeNameUpper, @"DECIMAL\((\d+),(\d+)\)");
                int count = 10, precision = 6;

                if (match.Success)
                {
                    count = Convert.ToInt32(match.Groups[0]);
                    precision = Convert.ToInt32(match.Groups[1]);
                }
                return new DbDoubleDataType(count, precision);
            }
            else
                return new DbCharDataType();
        }

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
