using System.Data.SQLite;
using System.IO;

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
        /// Initialize
        /// </summary>
        /// <param name="connectionString"></param>
        public override void Initialize(string connectionString)
        {
            Connection = new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// Initialize the database connection
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        public override void Initialize(string dbfilepath, string name)
        {
            base.Initialize(dbfilepath, name);

            Connection = new SQLiteConnection(string.Format("Data Source={0}.{1}",
                Path.Combine(FullPath, DbName), FileExtension));
        }

        /// <summary>
        /// Constructor with db file path and name and connection string
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public override void Initialize(string dbfilepath, string name, string connectionString)
        {
            Initialize(dbfilepath, name);
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
