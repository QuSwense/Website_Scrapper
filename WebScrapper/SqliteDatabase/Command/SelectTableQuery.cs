using System;
using System.Collections.Generic;
using System.Diagnostics;
using SqliteDatabase.Model;

namespace SqliteDatabase.Command
{
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
        /// Start the sql generation
        /// </summary>
        public void GenerateTableScrapUid(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            SQL = string.Format("SELECT MAX(uid) FROM tblscrpmdt");
        }

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

        internal void GenerateValidation(string table, string column, string value)
        {
            SQL = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2}", table, column,
                DataTypeContextHelper.GetQueryFormat(value, EConfigDbDataType.STRING));
        }

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

        #endregion Generate
    }
}
