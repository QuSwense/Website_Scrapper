﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using WebScrapper.Db.Config;

namespace WebScrapper.Db.Ctx
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
    /// <typeparam name="TColDbConfig">The column database configuration data</typeparam>
    /// <typeparam name="TDynCol">The Column data</typeparam>
    public class DynamicDbCommand<TDynTable,
        TDynRow,
            TDynColMetadata,
            TDbConnection,
            TColDbConfig,
            TDynCol>
        where TDynTable : DynamicTable<
            TDynRow,
            TDynColMetadata,
            TColDbConfig,
            TDynCol
            >, new()
        where TDbConnection : DbConnection
        where TDynRow : DynamicRow, new()
        where TDynColMetadata : DynamicColumnMetadata, new()
        where TColDbConfig : ColumnDbConfig
        where TDynCol : DynamicColumn<TDynColMetadata>, new()
    {
        /// <summary>
        /// The reference to the database context
        /// </summary>
        protected IDbContext dbContext { get; set; }

        /// <summary>
        /// The sql statement of the command executed in the database
        /// </summary>
        public string SQL { get; protected set; }

        /// <summary>
        /// Connection context object
        /// </summary>
        public TDbConnection ConnectionCtx { get; protected set; }

        public DynamicDbCommand() { }

        /// <summary>
        /// Constructor with database conetxt object and connection object
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="connectionCtx"></param>
        public DynamicDbCommand(IDbContext dbContext, TDbConnection connectionCtx)
        {
            this.dbContext = dbContext;
            ConnectionCtx = connectionCtx;
        }

        /// <summary>
        /// Create table command
        /// </summary>
        /// <param name="dynTable"></param>
        public void CreateTable(TDynTable dynTable)
        {
            List<string> colDefList = new List<string>();
            List<string> pkList = new List<string>();

            foreach (var item in dynTable.Headers)
            {
                TDynColMetadata header = item.Value;

                colDefList.Add(header.ColumnName + " " +
                    dbContext.GetDataType(header.DataType.GetType()) +
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

        public virtual void ExecuteDDL() { }
        public virtual DbDataReader ExecuteDML() { return null; }

        public virtual DbDataReader LoadMetadata(string name) { return null; }
    }
}
