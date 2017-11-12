using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WebCommon.Extn;

namespace DynamicDatabase.Command
{
    /// <summary>
    /// A helper class to create sql for insert data into table
    /// </summary>
    public class InsertOrUpdateIntoTableQuery : ICommandQuery
    {
        #region Properties

        /// <summary>
        /// The parent DbCommand class
        /// </summary>
        private IDbCommand dbCommand;

        /// <summary>
        /// Column definition list
        /// </summary>
        private List<string> columnInsertions;

        /// <summary>
        /// List of unique Keys
        /// </summary>
        private List<string> columnValues;

        /// <summary>
        /// List of column names
        /// </summary>
        private List<string> columnNames;

        /// <summary>
        /// The SQL statement
        /// </summary>
        public List<string> SQLs { get; protected set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The constructor with command class
        /// </summary>
        public InsertOrUpdateIntoTableQuery(IDbCommand dbCommand)
        {
            this.dbCommand = dbCommand;
            columnInsertions = new List<string>();
            columnValues = new List<string>();
            columnNames = new List<string>();
            SQLs = new List<string>();
        }

        #endregion Constructor

        #region Generate

        /// <summary>
        /// Start the sql generation
        /// </summary>
        public void GenerateAllColumns(IDbTable dynTable)
        {
            GenerateAllColumns(dynTable, dbCommand.InsertOrReplaceQueryString);
        }

        /// <summary>
        /// Start the sql generation
        /// </summary>
        public void GenerateOnlyInsertAllColumns(IDbTable dynTable)
        {
            GenerateAllColumns(dynTable, dbCommand.InsertQueryString);
        }

        /// <summary>
        /// A common method for generation
        /// </summary>
        /// <param name="dynTable"></param>
        /// <param name="queryFormat"></param>
        private void GenerateAllColumns(IDbTable dynTable, string queryFormat)
        {
            Debug.Assert(dbCommand != null);
            Debug.Assert(dbCommand.DbContext != null);
            Debug.Assert(dbCommand.DbContext.DbDataType != null);
            Debug.Assert(dynTable != null);
            Debug.Assert(dynTable.Headers != null && dynTable.Headers.Count() > 0);
            Debug.Assert(dynTable.Headers.ByNames != null && dynTable.Headers.ByNames.Count > 0);
            Debug.Assert(dynTable.Headers.ByIndices != null && dynTable.Headers.ByIndices.Count > 0);
            Debug.Assert(dynTable.Headers.ByNames.Count() == dynTable.Headers.ByIndices.Count());
            Debug.Assert(dynTable.Rows != null);

            for(int col = 0; col < dynTable.Headers.Count(); ++col)
            {
                columnNames.Add(dynTable.Headers[col].ColumnName);
            }

            for (int row = 0; row < dynTable.Rows.Count(); row++)
            {
                if (dynTable.Rows[row].IsDirty)
                {
                    List<string> rowData = new List<string>();
                    for (int col = 0; col < dynTable.Headers.Count(); col++)
                    {
                        string colValue = dbCommand.DbContext.DbDataType.GetValue(
                            dynTable.Rows[row].TryGetValue(col), Normalize);
                        rowData.Add(colValue);
                    }
                    columnValues.Add(string.Join(",", rowData));
                }
            }

            if (dbCommand.MaxInsertCriteriaAllowed > 0)
            {
                int counter = 0;
                while (counter < dbCommand.MaxInsertCriteriaAllowed)
                {
                    List<string> buffered = columnValues.GetRange(counter, dbCommand.MaxInsertCriteriaAllowed).ToList();
                    SQLs.Add(
                    queryFormat.Inject(new Dictionary<string, string>()
                    {
                        { "TableName", dynTable.TableName },
                        { "ColumnList", string.Join(",", columnNames) },
                        { "Values", string.Join(")," + Environment.NewLine + "(", buffered) }
                    }));

                    counter += dbCommand.MaxInsertCriteriaAllowed;
                }
            }
            else
            {
                SQLs.Add(
                    queryFormat.Inject(new Dictionary<string, string>()
                    {
                        { "TableName", dynTable.TableName },
                        { "ColumnList", string.Join(",", columnNames) },
                        { "Values", string.Join(")," + Environment.NewLine + "(", columnValues) }
                    }));
            }
        }

        /// <summary>
        /// Check and sanitize the value for SQL query
        /// </summary>
        /// <param name="colValue"></param>
        /// <returns></returns>
        public string Normalize(string colValue)
        {
            colValue = colValue.Replace("'", "''");
            return colValue;
        }

        #endregion Generate
    }
}
