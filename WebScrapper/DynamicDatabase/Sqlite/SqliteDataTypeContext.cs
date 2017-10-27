using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DynamicDatabase.Sqlite
{
    public class SqliteDataTypeContext : DataTypeContext
    {
        /// <summary>
        /// Get the sqlitre data tyupe from the <see cref="Type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override string GetDataType(Type type)
        {
            if (type == typeof(string) || type == typeof(DbCharDataType))
                return "TEXT";
            else if (type == typeof(int) || type == typeof(DbIntDataType))
                return "INTEGER";
            else if (type == typeof(double) || type == typeof(DbDoubleDataType))
                return "REAL";
            else if (type == typeof(DateTime) || type == typeof(DbDateTimeDataType))
                return "NUMERIC";
            else
                return "TEXT";
        }

        /// <summary>
        /// Parses the data type from a string and returns the generic <see cref="DbDataType"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public override DbDataType ParseDataType(string typeName)
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
    }
}
