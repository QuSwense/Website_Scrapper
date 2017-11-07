using System;
using System.Data.Common;

namespace DynamicDatabase.Interfaces
{
    public interface IDbCommand : IDisposable
    {
        #region Properties

        /// <summary>
        /// The reference to the database context
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// The sql statement of the command executed in the database
        /// </summary>
        string SQL { get; }

        /// <summary>
        /// Some Databases have the maximum number of VALUES query that can be executed for a INSERT
        /// statement in a batch
        /// </summary>
        int MaxInsertCriteriaAllowed { get; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor with database conetxt object and connection object
        /// </summary>
        /// <param name="dbContext"></param>
        void Initialize(IDbContext dbContext);

        #endregion Constructor

        #region Create

        /// <summary>
        /// Create table command
        /// </summary>
        /// <param name="dynTable"></param>
        void CreateTable(IDbTable dynTable);

        /// <summary>
        /// Remove the table
        /// </summary>
        /// <param name="tableName"></param>
        void RemoveTable(string tableName);

        /// <summary>
        /// Insert a row in a table
        /// </summary>
        /// <param name="dbTable"></param>
        /// <param name="colIndexData"></param>
        void Insert(IDbTable dbTable, string[] colIndexData);

        /// <summary>
        /// Insert or replace all the table data in the Database
        /// </summary>
        /// <param name="dbTable"></param>
        void InsertOrReplace(IDbTable dbTable);

        #endregion Create

        #region Load

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

        #endregion Load

        #region Execute

        /// <summary>
        /// Execute a Data definition Language Query
        /// </summary>
        void ExecuteDDL();

        /// <summary>
        /// Execute a Data Manipulation Language Query
        /// </summary>
        /// <returns></returns>
        DbDataReader ExecuteDML();

        #endregion Execute
    }
}
