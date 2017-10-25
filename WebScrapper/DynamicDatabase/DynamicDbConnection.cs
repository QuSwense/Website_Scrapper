using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace DynamicDatabase
{
    /// <summary>
    /// Dynamic database connection class
    /// </summary>
    public class DynamicDbConnection : IDynamicDbConnection
    {
        /// <summary>
        /// Connection object
        /// </summary>
        public DbConnection Connection { get; set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string DbName { get; protected set; }

        /// <summary>
        /// The full local path of the database file
        /// </summary>
        public string FullPath { get; set; }

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
        /// Constructor with db database file and name
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        public DynamicDbConnection(string dbfilepath, string name)
        {
            DbName = name;
            FullPath = dbfilepath;
            Connection = DynamicDbFactory.Create<DbConnection>(dbfilepath, name);
        }

        /// <summary>
        /// Constructor with db file path and name and connection string
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public DynamicDbConnection(string dbfilepath, string name, string connectionString)
        {
            DbName = name;
            FullPath = dbfilepath;
            Connection = DynamicDbFactory.Create<DbConnection>(dbfilepath, name, connectionString);
        }

        /// <summary>
        /// Construct the database context using the connection string
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="connectionCtx"></param>
        public DynamicDbConnection(string connectionString)
        {
            Connection = DynamicDbFactory.Create<DbConnection>(connectionString);
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
        public virtual void Open() { }

        /// <summary>
        /// Close a connection
        /// </summary>
        public virtual void Close() { }

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
