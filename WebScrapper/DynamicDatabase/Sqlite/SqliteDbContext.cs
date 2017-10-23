using DynamicDatabase;
using DynamicDatabase.Config;
using DynamicDatabase.Default;
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
    public class SqliteDbContext : DynamicDbContext<
        DTable,
        SqliteDbCommand,
        SQLiteConnection,
        DynamicRow,
        DColumnMetadata
        >
    {
        /// <summary>
        /// The extension of the database file
        /// </summary>
        public override string FileExtension { get { return "sqlite"; } }

        /// <summary>
        /// Get the full path of the sqlite file path
        /// </summary>
        public override string FullDbFileName
        {
            get { return Path.Combine(FullPath, DbName) + "." + FileExtension; }
        }

        /// <summary>
        /// Constructor with the database name with the file path
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        public SqliteDbContext(string dbfilepath, string name) 
            : base(dbfilepath, name)
        {
            string connection = string.Format("Data Source={0}.{1}", 
                Path.Combine(FullPath, DbName), FileExtension);
            ConnectionCtx = new SQLiteConnection(connection);
            DbCommand = new SqliteDbCommand(this, ConnectionCtx);
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
