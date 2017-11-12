using DynamicDatabase.Command;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Types;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DynamicDatabase
{
    /// <summary>
    /// The database command class.
    /// This class is used internally by the <see cref="DynamicDbContext"/> class to build and execute
    /// any DDL or DML commands.
    /// </summary>
    /// <typeparam name="TDynTable">Referecne to the Table class</typeparam>
    /// <typeparam name="TDynRow">Reference to the row of the table</typeparam>
    /// <typeparam name="TDynColMetadata">The column metadata class</typeparam>
    /// <typeparam name="TDbConnection">The database connection class</typeparam>
    /// <typeparam name="TDynCol">The Column data</typeparam>
    public class DynamicDbCommand : IDbCommand
    {
        #region Properties

        /// <summary>
        /// The reference to the database context
        /// </summary>
        public IDbContext DbContext { get; protected set; }

        /// <summary>
        /// The sql statement of the command executed in the database
        /// </summary>
        public List<string> SQLs { get; protected set; }

        /// <summary>
        /// Some Databases have the maximum number of VALUES query that can be executed for a INSERT
        /// statement in a batch
        /// </summary>
        public virtual int MaxInsertCriteriaAllowed { get { return -1; } }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DynamicDbCommand() { }

        /// <summary>
        /// Constructor with database conetxt object and connection object
        /// </summary>
        /// <param name="dbContext"></param>
        public void Initialize(IDbContext dbContext)
        {
            this.DbContext = dbContext;
            SQLs = new List<string>();
        }

        #endregion Constructor

        #region Create

        /// <summary>
        /// Create table command
        /// </summary>
        /// <param name="dynTable"></param>
        public virtual void CreateTable(IDbTable dynTable)
        {
            CreateTableQuery createTableQueryObj = new CreateTableQuery(this);
            createTableQueryObj.Generate(dynTable);
            SetSQL(createTableQueryObj.SQLs);
            ExecuteDDL();
        }

        /// <summary>
        /// Remove the table
        /// </summary>
        /// <param name="tableName"></param>
        public virtual void RemoveTable(string tableName)
        {
            SetSQL(string.Format(DeleteTableString, tableName));
            ExecuteDDL();
        }

        /// <summary>
        /// Insert or replace all the table data in the Database
        /// </summary>
        /// <param name="dbTable"></param>
        public virtual void InsertOrReplace(IDbTable dbTable)
        {
            InsertOrUpdateIntoTableQuery insorUpdtIntoTableQueryObj = new InsertOrUpdateIntoTableQuery(this);
            insorUpdtIntoTableQueryObj.GenerateAllColumns(dbTable);
            SetSQL(insorUpdtIntoTableQueryObj.SQLs);

            ExecuteDDL();

            for (int row = 0; row < dbTable.Rows.ByIndices.Count; row++) dbTable.Rows[row].IsDirty = false;
        }

        /// <summary>
        /// Insert all the table data in the Database
        /// </summary>
        /// <param name="dbTable"></param>
        public virtual void Insert(IDbTable dbTable)
        {
            InsertOrUpdateIntoTableQuery insorUpdtIntoTableQueryObj = new InsertOrUpdateIntoTableQuery(this);
            insorUpdtIntoTableQueryObj.GenerateOnlyInsertAllColumns(dbTable);
            SetSQL(insorUpdtIntoTableQueryObj.SQLs);

            for (int row = 0; row < dbTable.Rows.ByIndices.Count; row++) dbTable.Rows[row].IsDirty = false;
        }

        /// <summary>
        /// Check the existence of the table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool TableExists(string name)
        {
            SetSQL(string.Format(TableExistenceString, name));
            DbDataReader dbReader = ExecuteDML();

            if (dbReader.HasRows) return true;
            return false;
        }

        #endregion Create

        #region Load

        /// <summary>
        /// Select all data of the table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual DbDataReader LoadData(string name)
        {
            SelectTableQuery selectTableQueryObj = new SelectTableQuery(this);
            selectTableQueryObj.GenerateAll(name);
            SetSQL(selectTableQueryObj.SQLs);
            return ExecuteDML();
        }

        /// <summary>
        /// Select all data of the table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual DbDataReader LoadTableMetadata(string name) { return null; }

        /// <summary>
        /// Load metadata
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual DbDataReader LoadMetadata(string name) { return null; }

        #endregion Load

        #region Execute

        /// <summary>
        /// Execute a Data definition Language Query
        /// </summary>
        public virtual void ExecuteDDL() { }

        /// <summary>
        /// Execute a Data Manipulation Language Query
        /// </summary>
        /// <returns></returns>
        public virtual DbDataReader ExecuteDML() { return null; }

        #endregion Execute

        #region Format

        /// <summary>
        /// The Not null string
        /// </summary>
        public virtual string NotNullString { get { return "NOT NULL"; } }

        /// <summary>
        /// The Unique string
        /// </summary>
        public virtual string UniqueString { get { return "UNQIUE"; } }

        /// <summary>
        /// Get the format for the column definition line
        /// </summary>
        /// <returns></returns>
        public virtual string ColumnDefinitionString
        { get { return "{Name} {DataType} {ConstraintNotNull} {ConstraintUnique}"; } }

        /// <summary>
        /// Get the format for the primary key contraint
        /// </summary>
        /// <returns></returns>
        public virtual string PrimaryKeyConstraintString { get { return ", PRIMARY KEY ({0})"; } }

        /// <summary>
        /// Get the format for the create table
        /// </summary>
        /// <returns></returns>
        public virtual string CreateTableString { get { return "CREATE TABLE {TableName} ( {ColDefs} {PKs} )"; } }

        /// <summary>
        /// Get the format for the delete table
        /// </summary>
        /// <returns></returns>
        public virtual string DeleteTableString { get { return "DROP TABLE {0}"; } }

        /// <summary>
        /// Gets the Insert query format for the database
        /// </summary>
        /// <returns></returns>
        public virtual string InsertQueryString { get { return "INSERT INTO {0} VALUES ( {1} )"; } }

        /// <summary>
        /// Gets the Insert query format for the database by column names
        /// </summary>
        /// <returns></returns>
        public virtual string InsertByColumnQueryString { get { return "INSERT INTO TABLE {0} ( {1} ) VALUES ( {2} )"; } }

        /// <summary>
        /// Gets the Insert query format for the database
        /// </summary>
        /// <returns></returns>
        public virtual string InsertOrReplaceQueryString { get { return ""; } }

        /// <summary>
        /// Gets the select query format for the database
        /// </summary>
        /// <returns></returns>
        public virtual string SelectQueryString { get { return "SELECT {Columns} FROM {TableName}"; } }

        /// <summary>
        /// Gets the information / metdata about a table from the database
        /// </summary>
        public virtual string TableInformationString { get { return ""; } }

        /// <summary>
        /// Gets the existence of a table from the database
        /// </summary>
        public virtual string TableExistenceString { get { return ""; } }

        #endregion Format

        #region Helper

        /// <summary>
        /// Set the sql list
        /// </summary>
        /// <param name="sqls"></param>
        protected void SetSQL(List<string> sqls)
        {
            SQLs = new List<string>(sqls);
        }

        /// <summary>
        /// Set the sql list
        /// </summary>
        /// <param name="sqls"></param>
        protected void SetSQL(string sql)
        {
            SQLs.Clear();
            SQLs.Add(sql);
        }

        #endregion Helper

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

        /// <summary>
        /// This code added to correctly implement the disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
