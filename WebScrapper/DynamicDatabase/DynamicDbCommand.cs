using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

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
        /// <summary>
        /// The reference to the database context
        /// </summary>
        protected IDbContext dbContext { get; set; }

        /// <summary>
        /// The sql statement of the command executed in the database
        /// </summary>
        public string SQL { get; protected set; }

        public virtual int MaxInsertCriteriaAllowed { get { return -1; } }

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
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Create table command
        /// </summary>
        /// <param name="dynTable"></param>
        public void CreateTable(IDbTable dynTable)
        {
            List<string> colDefList = new List<string>();
            List<string> pkList = new List<string>();

            foreach (var item in dynTable.Headers.ByNames)
            {
                IColumnMetadata header = item.Value;

                colDefList.Add(header.ColumnName + " " +
                    dbContext.DbDataType.GetDataType(header.DataType.GetType()) +
                    (((header.Constraint & EColumnConstraint.NOTNULL) == EColumnConstraint.NOTNULL) ? " NOT NULL" : "") +
                    (((header.Constraint & EColumnConstraint.UNQIUE) == EColumnConstraint.UNQIUE) ? " UNIQUE" : ""));

                if ((header.Constraint & EColumnConstraint.PRIMARYKEY) == EColumnConstraint.PRIMARYKEY)
                    pkList.Add(header.ColumnName);
            }

            SQL = string.Format("CREATE TABLE {0} ( {1} {2} )", dynTable.TableName, string.Join(",", colDefList),
                    ((pkList.Count > 0) ? string.Format(", PRIMARY KEY ({0})", string.Join(",", pkList)) : "")
                    );

            ExecuteDDL();
        }

        /// <summary>
        /// Select all data of the table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DbDataReader LoadData(string name)
        {
            SQL = string.Format("SELECT * FROM {0}", name);
            return ExecuteDML();
        }

        /// <summary>
        /// Select all data of the table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DbDataReader LoadTableMetadata(string name)
        {
            SQL = string.Format("PRAGMA table_info('{0}')", name);
            return ExecuteDML();
        }

        /// <summary>
        /// Execute a Data definition Language Query
        /// </summary>
        public virtual void ExecuteDDL() { }

        /// <summary>
        /// Execute a Data Manipulation Language Query
        /// </summary>
        /// <returns></returns>
        public virtual DbDataReader ExecuteDML() { return null; }

        /// <summary>
        /// Load metadata
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual DbDataReader LoadMetadata(string name) { return null; }

        /// <summary>
        /// Insert a row in a table
        /// </summary>
        /// <param name="dbTable"></param>
        /// <param name="colIndexData"></param>
        public virtual void Insert(IDbTable dbTable, string[] colIndexData)
        {
            List<string> colInsList = new List<string>();
            List<string> valueList = new List<string>();

            for (int i = 0; i < colIndexData.Length; i++)
            {
                colInsList.Add(dbTable.Headers[i].ColumnName);
            }

            for (int i = 0; i < colIndexData.Length; i++)
            {
                valueList.Add(DbDataTypeHelper.GetValue(dbTable.Headers[i].DataType, colIndexData[i]));
            }

            SQL = string.Format("INSERT INTO TABLE {0} ( {1} ) VALUES ( {2} )", dbTable.TableName, 
                string.Join(",", colInsList), string.Join(",", valueList)
                    );

            ExecuteDDL();
        }

        /// <summary>
        /// Insert or replace all the table data in the Database
        /// </summary>
        /// <param name="dbTable"></param>
        public void InsertOrReplace(IDbTable dbTable)
        {
            List<string> valueList = new List<string>();

            for (int row = 0; row < dbTable.Rows.ByIndices.Count; row++)
            {
                List<string> rowData = new List<string>();
                for (int col = 0; col < dbTable.Rows[row].Columns.ByIndices.Count; col++)
                {
                    rowData.Add(DbDataTypeHelper.GetValue(dbTable.Headers[col].DataType, dbTable.Rows[row].Columns[col]));
                }

                valueList.Add(string.Join(",", rowData));
            }

            if(MaxInsertCriteriaAllowed > 0)
            {
                int counter = 0;
                while(counter < MaxInsertCriteriaAllowed)
                {
                    List<string> buffered = valueList.GetRange(counter, MaxInsertCriteriaAllowed).ToList();
                    SQL = string.Format("INSERT INTO TABLE {0} VALUES ( {2} )", dbTable.TableName,
                        string.Join("),(", buffered)
                    );

                    ExecuteDDL();
                    counter += MaxInsertCriteriaAllowed;
                }
            }
            else
            {
                SQL = string.Format("INSERT INTO TABLE {0} VALUES ( {2} )", dbTable.TableName,
                        string.Join("),(", valueList)
                    );
                ExecuteDDL();
            }
            
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Connection != null)
                    {
                        Connection.Dispose();
                    }
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
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
