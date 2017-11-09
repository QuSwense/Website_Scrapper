using System;
using System.Collections.Generic;
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
        List<string> SQLs { get; }

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
        /// Insert or replace all the table data in the Database
        /// </summary>
        /// <param name="dbTable"></param>
        void InsertOrReplace(IDbTable dbTable);

        /// <summary>
        /// Insert all the table data in the Database
        /// </summary>
        /// <param name="dbTable"></param>
        void Insert(IDbTable dbTable);

        /// <summary>
        /// Check the existence of the table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool TableExists(string name);

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

        #region Format

        /// <summary>
        /// The Not null string
        /// </summary>
        string NotNullString { get; }

        /// <summary>
        /// The Unique string
        /// </summary>
        string UniqueString { get; }

        /// <summary>
        /// Get the format for the column definition line
        /// </summary>
        string ColumnDefinitionString { get; }

        /// <summary>
        /// Get the format for the primary key contraint
        /// </summary>
        string PrimaryKeyConstraintString { get; }

        /// <summary>
        /// Get the format for the create table
        /// </summary>
        string CreateTableString { get; }

        /// <summary>
        /// Get the format for the delete table
        /// </summary>
        string DeleteTableString { get; }

        /// <summary>
        /// Gets the Insert query format for the database
        /// </summary>
        string InsertQueryString { get; }

        /// <summary>
        /// Gets the Insert query format for the database by column names
        /// </summary>
        string InsertByColumnQueryString { get; }

        /// <summary>
        /// Gets the Insert or replace query format for the database
        /// </summary>
        string InsertOrReplaceQueryString { get; }

        /// <summary>
        /// Gets the select query format for the database
        /// </summary>
        string SelectQueryString { get; }

        /// <summary>
        /// Gets the information / metdata about a table from the database
        /// </summary>
        string TableInformationString { get; }

        /// <summary>
        /// Gets the existence of a table from the database
        /// </summary>
        string TableExistenceString { get; }

        #endregion Format
    }
}
