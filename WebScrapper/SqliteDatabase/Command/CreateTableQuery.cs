using SqliteDatabase.Model;
using System.Collections.Generic;

namespace SqliteDatabase.Command
{
    /// <summary>
    /// This class is used internally by the command class to create SQl for Create table
    /// </summary>
    public class CreateTableQuery
    {
        #region Properties
        
        /// <summary>
        /// The SQL statement
        /// </summary>
        public List<string> SQLs { get; protected set; }
        
        #endregion Properties

        #region Constructor

        /// <summary>
        /// The constructor with command class
        /// </summary>
        public CreateTableQuery()
        {
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
                    List<string> tmpColAttributes = new List<string>
                    {
                        colKv.Key,
                        DataTypeContextHelper.GetSqliteType(colKv.Value.DataType)
                    };
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
