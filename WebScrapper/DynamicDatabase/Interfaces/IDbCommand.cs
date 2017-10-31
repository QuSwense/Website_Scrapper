using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Interfaces
{
    public interface IDbCommand : IDisposable
    {
        /// <summary>
        /// The sql statement of the command executed in the database
        /// </summary>
        string SQL { get; }

        /// <summary>
        /// Connection context object
        /// </summary>
        IDynamicDbConnection Connection { get; }

        /// <summary>
        /// Constructor with database conetxt object and connection object
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="connectionCtx"></param>
        void Initialize(IDbContext dbContext, IDynamicDbConnection connectionCtx = null);

        /// <summary>
        /// Create table command
        /// </summary>
        /// <param name="dynTable"></param>
        void CreateTable(IDbTable dynTable);

        /// <summary>
        /// Execute delete table command
        /// </summary>
        /// <param name="dynTable"></param>
        void RemoveTable(IDbTable dynTable);

        /// <summary>
        /// Execute delete table command from the table name
        /// </summary>
        /// <param name="tablename"></param>
        void RemoveTable(string tablename);

        /// <summary>
        /// Select all data of the table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DbDataReader LoadData(string name);

        /// <summary>
        /// Select all data of the table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DbDataReader LoadTableMetadata(string name);

        /// <summary>
        /// Execute a Data definition Language Query
        /// </summary>
        void ExecuteDDL();

        /// <summary>
        /// Execute a Data Manipulation Language Query
        /// </summary>
        /// <returns></returns>
        DbDataReader ExecuteDML();

        /// <summary>
        /// Load metadata
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DbDataReader LoadMetadata(string name);
    }
}
