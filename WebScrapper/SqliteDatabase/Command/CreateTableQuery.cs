using SqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WebCommon.Extn;

namespace SqliteDatabase.Command
{
    /// <summary>
    /// This class is used internally by the command class to create SQl for Create table
    /// </summary>
    public class CreateTableQuery
    {
        #region Properties
        
        /// <summary>
        /// Column definition list
        /// </summary>
        private List<string> columnDefinitions;

        /// <summary>
        /// List of unique Keys
        /// </summary>
        private List<string> primaryKeys;

        /// <summary>
        /// The SQL statement
        /// </summary>
        public List<string> SQLs { get; protected set; }

        /// <summary>
        /// Get the format for the column definition line
        /// </summary>
        /// <returns></returns>
        public virtual string ColumnDefinitionString
        { get { return "{Name} {DataType} {ConstraintNotNull} {ConstraintUnique}"; } }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The constructor with command class
        /// </summary>
        public CreateTableQuery()
        {
            columnDefinitions = new List<string>();
            primaryKeys = new List<string>();
            SQLs = new List<string>();
        }

        #endregion Constructor

        #region Generate

        /// <summary>
        /// Start the sql generation
        /// </summary>
        public void Generate(DbTablesDefinitionModel tableColumnConfigs)
        {
            foreach (var tableKv in tableColumnConfigs)
            {
                List<string> colLists = new List<string>();
                List<string> pkCols = new List<string>();
                foreach (var colKv in tableKv.Value)
                {
                    List<string> tmpColAttributes = new List<string>();
                    tmpColAttributes.Add(colKv.Key);
                    tmpColAttributes.Add(DataTypeContextHelper.GetSqliteType(colKv.Value.DataType));
                    if (colKv.Value.IsPrimaryKey)
                        pkCols.Add(colKv.Key);
                    colLists.Add(string.Join(" ", tmpColAttributes));
                }

                string pkList = null;

                if(pkCols.Count > 0) pkList = string.Join(",", pkCols);

                SQLs.Add(string.Format("CREATE TABLE IF NOT EXISTS {0} ({1} {2})",
                    tableKv.Key,
                    string.Join(",", colLists),
                    (!string.IsNullOrEmpty(pkList))? string.Format(", PRIMARY KEY ({0})", pkList) : ""
                    ));
            }
        }

        #endregion Generate
    }
}
