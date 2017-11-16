using SqliteDatabase.Model;
using System;
using System.Collections.Generic;

namespace SqliteDatabase.Command
{
    /// <summary>
    /// A helper class to create sql for insert data into table
    /// </summary>
    public class InsertOrUpdateIntoTableQuery
    {
        #region Properties
        
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
        public string SQL { get; protected set; }

        /// <summary>
        /// Gets the Insert query format for the database
        /// </summary>
        /// <returns></returns>
        public virtual string InsertQueryString { get { return "INSERT INTO {0} VALUES ( {1} )"; } }

        /// <summary>
        /// Gets the Insert query format for the database by column names
        /// </summary>
        /// <returns></returns>
        public virtual string InsertOrReplaceByColumnQueryString { get { return "INSERT OR REPLACE INTO {0} ( {1} ) VALUES ( {2} )"; } }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The constructor with command class
        /// </summary>
        public InsertOrUpdateIntoTableQuery()
        {
            columnInsertions = new List<string>();
            columnValues = new List<string>();
            columnNames = new List<string>();
        }

        #endregion Constructor

        #region Generate
        
        /// <summary>
        /// Generate
        /// </summary>
        /// <param name="tableMetadatas"></param>
        public void Generate(DbTablesMetdataDefinitionModel tableMetadatas)
        {
            foreach (var kv in tableMetadatas)
            {
                columnValues.Add(string.Format("'{0}','{1}','{2}'", 
                    kv.Key, kv.Value.Display, kv.Value.Reference));
            }

            SQL = string.Format(InsertQueryString, "mdt",
                string.Join(")," + Environment.NewLine + "(", columnValues));
        }

        public void Generate(TableScrapMetadataModel tblScrapMdtModel, int nextUid)
        {
            SQL = string.Format(InsertQueryString, "tblscrpmdt",
                    string.Format("{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'", nextUid,
                            tblScrapMdtModel.Name,
                            tblScrapMdtModel.Url1,
                            tblScrapMdtModel.Url2,
                            tblScrapMdtModel.Url3,
                            tblScrapMdtModel.Url4,
                            DataTypeContextHelper.NormalizeValue(tblScrapMdtModel.XPath1),
                            DataTypeContextHelper.NormalizeValue(tblScrapMdtModel.XPath2),
                            DataTypeContextHelper.NormalizeValue(tblScrapMdtModel.XPath3),
                            DataTypeContextHelper.NormalizeValue(tblScrapMdtModel.XPath4)
                            ));
        }

        internal void Generate(List<ColumnScrapMetadataModel> colScrapMdtModels)
        {
            List<string> rowValues = new List<string>();

            for (int i = 0; i < colScrapMdtModels.Count; i++)
            {
                rowValues.Add(string.Format("'{0}','{1}','{2}','{3}',{4},{5}",
                    colScrapMdtModels[i].ColumnName,
                    colScrapMdtModels[i].DisplayName,
                    "",
                    DataTypeContextHelper.NormalizeValue(colScrapMdtModels[i].XPath),
                    colScrapMdtModels[i].Index,
                    colScrapMdtModels[i].Uid
                    ));
            }

            SQL = string.Format(InsertQueryString, "colscrpmdt",
                    string.Join("),(", rowValues));
        }

        internal void Generate(string name, List<ColumnModel> colValueMappings)
        {
            List<string> colNames = new List<string>();
            List<string> colValues = new List<string>();

            for (int i = 0; i < colValueMappings.Count; i++)
            {
                colNames.Add(colValueMappings[i].Name);
                colValues.Add(
                    DataTypeContextHelper.GetQueryFormat(
                        DataTypeContextHelper.NormalizeValue(colValueMappings[i].Value), colValueMappings[i].DataType));
            }

            SQL = string.Format(InsertOrReplaceByColumnQueryString, name,
                    string.Join(",", colNames),
                    string.Join(",", colValues));
        }

        internal void Generate(string name, List<DynamicTableDataInsertModel> row)
        {
            List<string> colNames = new List<string>();
            List<string> colValues = new List<string>();

            for (int i = 0; i < row.Count; i++)
            {
                colNames.Add(row[i].Name);
                colValues.Add(
                    DataTypeContextHelper.GetQueryFormat(
                        DataTypeContextHelper.NormalizeValue(row[i].Value), row[i].DataType));
            }

            SQL = string.Format(InsertOrReplaceByColumnQueryString, name,
                    string.Join(",", colNames),
                    string.Join(",", colValues));
        }

        #endregion Generate
    }
}
