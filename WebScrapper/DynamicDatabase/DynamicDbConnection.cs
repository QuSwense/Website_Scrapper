using DynamicDatabase.Interfaces;
using System.Data.Common;
using System.IO;

namespace DynamicDatabase
{
    /// <summary>
    /// Dynamic database connection class. It wraps the main <see cref="DbConnection"/>
    /// class. The <see cref="DynamicDbFactory"/> is used to create the new connection object
    /// using the needed class.
    /// </summary>
    public abstract class DynamicDbConnection : IDynamicDbConnection
    {
        /// <summary>
        /// The main db Connection object
        /// Use <see cref="DynamicDbFactory"/> to create the main class
        /// </summary>
        public DbConnection Connection { get; protected set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string DbName { get; protected set; }

        /// <summary>
        /// The full local path of the database file
        /// </summary>
        public string FullPath { get; protected set; }

        /// <summary>
        /// The extension of the database file
        /// </summary>
        public virtual string FileExtension { get; protected set; }

        /// <summary>
        /// Get the full path of the database file including the name
        /// </summary>
        public virtual string FullDbFileName
        {
            get { return Path.Combine(FullPath, DbName); }
        }

        /// <summary>
        /// Gets a string that represents the version of the server to which the object is connected
        /// </summary>
        public virtual string ServerVersion
        {
            get { return Connection.ServerVersion; }
        }

        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        public virtual string DataSource
        {
            get { return Connection.DataSource; }
        }

        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the
        /// database name specified in the connection string before the connection is opened.
        /// </summary>
        public virtual string Database
        {
            get { return Connection.Database; }
        }

        /// <summary>
        /// Gets the time to wait while establishing a connection before terminating 
        /// the attempt and generating an error.
        /// </summary>
        public virtual int ConnectionTimeout
        {
            get { return Connection.ConnectionTimeout; }
        }

        /// <summary>
        /// Gets or sets the string used to open the connection.
        /// </summary>
        public virtual string ConnectionString
        {
            get { return Connection.ConnectionString; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DynamicDbConnection() { }

        /// <summary>
        /// Constructor with db database file and name
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        public virtual void Initialize(string dbfilepath, string name)
        {
            DbName = name;
            FullPath = dbfilepath;
        }

        /// <summary>
        /// Constructor with db file path and name and connection string
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public virtual void Initialize(string dbfilepath, string name, string connectionString)
        {
            DbName = name;
            FullPath = dbfilepath;
        }

        /// <summary>
        /// Construct the database context using the connection string
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="connectionCtx"></param>
        public virtual void Initialize(string connectionString)
        {
            
        }

        /// <summary>
        /// Create the Sqlite database
        /// </summary>
        public virtual void CreateDatabase() { }

        /// <summary>
        /// Check if the sqlite database exists
        /// </summary>
        /// <returns></returns>
        public virtual bool DatabaseExists()
        {
            return false;
        }

        /// <summary>
        /// Delete the sqlite database
        /// </summary>
        public virtual void DeleteDatabase() { }

        /// <summary>
        /// Open a connection
        /// </summary>
        public virtual void Open()
        {
            Connection.Open();
        }

        /// <summary>
        /// Close a connection
        /// </summary>
        public virtual void Close()
        {
            Connection.Close();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                disposedValue = true;
            }
        }
        
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
