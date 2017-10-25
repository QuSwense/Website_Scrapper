using DynamicDatabase;
using DynamicDatabase.Config;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace WebScrapper.Db.Ctx
{
    /// <summary>
    /// A sqlite specific db context class which uses other state and command classes
    /// either generic or specific as per the requirement
    /// </summary>
    public class SqliteDbContext : DynamicDbContext
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
    }
}
