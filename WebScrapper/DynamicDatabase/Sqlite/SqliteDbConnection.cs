using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Sqlite
{
    public class SqliteDbConnection : DynamicDbConnection
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
        public SqliteDbConnection(string dbfilepath, string name) 
            : base(dbfilepath, name)
        {
            string connection = string.Format("Data Source={0}.{1}",
                Path.Combine(FullPath, DbName), FileExtension);
            Connection = new SQLiteConnection(connection);
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
    }
}
