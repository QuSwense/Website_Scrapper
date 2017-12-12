using System.Collections.Generic;
using System.Diagnostics;
using SqliteDatabase.Model;
using WebCommon.Extn;

namespace SqliteDatabase.Command
{
    /// <summary>
    /// A helper class for creating 'Select' type queries using different type of inputs
    /// </summary>
    public class SelectTableQuery
    {
        #region Properties
        
        /// <summary>
        /// Column definition list
        /// </summary>
        private List<string> columnDefinitions;
        
        /// <summary>
        /// The SQL statement
        /// </summary>
        public string SQL { get; protected set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The constructor with command class
        /// </summary>
        public SelectTableQuery()
        {
            columnDefinitions = new List<string>();
        }

        #endregion Constructor

        #region Generate

        /// <summary>
        /// A select query to get the maximum value of the unique identifier of the 'Table Scrap Metadata'
        /// </summary>
        public void GenerateTableScrapUid(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            SQL = string.Format("SELECT MAX(uid) FROM tblscrpmdt");
        }

        /// <summary>
        /// Generate a Select query with a where clause wqith the unique columns criteria
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        internal void GenerateUniqueRow(string name, List<DynamicTableDataInsertModel> row)
        {
            List<string> pkCols = new List<string>();

            for (int i = 0; i < row.Count; i++)
            {
                if (row[i].IsPk)
                {
                    pkCols.Add(string.Format("{0} = {1}", row[i].Name,
                        DataTypeContextHelper.GetQueryFormat(
                            DataTypeContextHelper.NormalizeValue(row[i].Value), row[i].DataType)));
                }
            }

            if (pkCols.Count <= 0) return;

            SQL = string.Format("SELECT * FROM {0} WHERE {1}", name,
                string.Join(" AND ", pkCols));
        }

        /// <summary>
        /// This generates a query to validate the existence of rows in a table / combination of tables
        /// This returns the count of rows found
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        internal void GenerateValidation(string table, string column, string value)
        {
            SQL = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2}", table, column,
                DataTypeContextHelper.GetQueryFormat(value, EConfigDbDataType.STRING));
        }

        /// <summary>
        /// Generate Single select data.
        /// It uses a single table or join of tables
        /// </summary>
        /// <param name="tableExists"></param>
        /// <param name="columnExists"></param>
        /// <param name="innerjoincriteria"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="result"></param>
        internal void GenerateSingle(string tableExists, string columnExists, string innerjoincriteria,
            string table, string column, string result)
        {
            if(string.IsNullOrEmpty(innerjoincriteria))
            {
                SQL = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", column, table, columnExists,
                DataTypeContextHelper.GetQueryFormat(result, EConfigDbDataType.STRING));
            }
            else
            {
                SQL = string.Format("SELECT {0} FROM {1}, {2} INNER JOIN {3} WHERE {2} = {3}", 
                    column, tableExists, table, innerjoincriteria, columnExists,
                DataTypeContextHelper.GetQueryFormat(result, EConfigDbDataType.STRING));
            }
            
        }

        /// <summary>
        /// Generate Query from the format and the data
        /// </summary>
        /// <param name="selectQueryFormat"></param>
        /// <param name="result"></param>
        public void GenerateQueryFromFormat(string selectQueryFormat, string result)
        {
            SQL = selectQueryFormat.InjectSingleValue("data",
                DataTypeContextHelper.GetQueryFormat(result, EConfigDbDataType.STRING));
        }

        #endregion Generate
    }
}
