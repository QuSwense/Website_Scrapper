using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using WebScrapper.Db.Config;
using WebScrapper.Db.Ctx.Db;

namespace WebScrapper.Db.Ctx
{
    /// <summary>
    /// A sqlite specific db context class which uses other state and command classes
    /// either generic or specific as per the requirement
    /// </summary>
    public class SqliteDbContext : DynamicDbContext<
        DTable,
        SqliteDbCommand,
        SQLiteConnection,
        DynamicRow,
        DColumnMetadata,
        ColumnDbConfig,
        DColumn
        >
    {
        /// <summary>
        /// The name of the sqlite database
        /// </summary>
        public string DbName { get; protected set; }

        /// <summary>
        /// The full local path of the database file
        /// </summary>
        public string FullPath { get; protected set; }

        /// <summary>
        /// Get the full path of the sqlite file path
        /// </summary>
        public string FullDbFileName
        {
            get
            {
                return Path.Combine(FullPath, DbName) + ".sqlite";
            }
        }

        /// <summary>
        /// Constructor with the database name with the file path
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        public SqliteDbContext(string dbfilepath, string name)
        {
            DbName = name;
            FullPath = dbfilepath;
            string connection = string.Format("Data Source={0}.sqlite", Path.Combine(FullPath, DbName));
            ConnectionCtx = new SQLiteConnection();
            DbCommand = new SqliteDbCommand(this, (SQLiteConnection)ConnectionCtx);
        }

        /// <summary>
        /// Create the Sqlite database
        /// </summary>
        public override void CreateDatabase()
        {
            SQLiteConnection.CreateFile(FullDbFileName);
        }

        /// <summary>
        /// Check if the sqlite database exists
        /// </summary>
        /// <returns></returns>
        public override bool DatabaseExists()
        {
            return File.Exists(FullDbFileName);
        }

        /// <summary>
        /// Delete the sqlite database
        /// </summary>
        public override void DeleteDatabase()
        {
            File.Delete(FullDbFileName);
        }

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
