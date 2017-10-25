using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Interfaces
{
    public interface IDynamicDbConnection : IDisposable
    {
        /// <summary>
        /// Connection object
        /// </summary>
        DbConnection Connection { get; set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        string DbName { get; }

        /// <summary>
        /// The full local path of the database file
        /// </summary>
        string FullPath { get; set; }

        /// <summary>
        /// The extension of the database file
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Get the full path of the database file including the name
        /// </summary>
        string FullDbFileName { get; }

        /// <summary>
        /// Create the Sqlite database
        /// </summary>
        void CreateDatabase();

        /// <summary>
        /// Check if the sqlite database exists
        /// </summary>
        /// <returns></returns>
        bool DatabaseExists();

        /// <summary>
        /// Delete the sqlite database
        /// </summary>
        void DeleteDatabase();

        /// <summary>
        /// Open a connection
        /// </summary>
        void Open();

        /// <summary>
        /// Close a connection
        /// </summary>
        void Close();
    }
}
