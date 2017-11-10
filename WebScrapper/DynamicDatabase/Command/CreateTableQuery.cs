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
    /// This class is used internally by the command class to create SQl for Create table
    /// </summary>
    public class CreateTableQuery : ICommandQuery
    {
        #region Properties

        /// <summary>
        /// The parent DbCommand class
        /// </summary>
        private IDbCommand dbCommand;

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

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The constructor with command class
        /// </summary>
        public CreateTableQuery(IDbCommand dbCommand)
        {
            this.dbCommand = dbCommand;
            columnDefinitions = new List<string>();
            primaryKeys = new List<string>();
            SQLs = new List<string>();
        }

        #endregion Constructor

        #region Generate

        /// <summary>
        /// Start the sql generation
        /// </summary>
        public void Generate(IDbTable dynTable)
        {
            Debug.Assert(dbCommand != null);
            Debug.Assert(dbCommand.DbContext != null);
            Debug.Assert(dbCommand.DbContext.DbDataType != null);
            Debug.Assert(dynTable != null);
            Debug.Assert(dynTable.Headers != null && dynTable.Headers.Count() > 0);
            Debug.Assert(dynTable.Headers.ByNames != null && dynTable.Headers.ByNames.Count > 0);
            Debug.Assert(dynTable.Headers.ByIndices != null && dynTable.Headers.ByIndices.Count > 0);
            Debug.Assert(dynTable.Headers.ByNames.Count() == dynTable.Headers.ByIndices.Count());

            foreach (var colHeaderkv in dynTable.Headers.ByNames)
            {
                IColumnMetadata colHeader = colHeaderkv.Value;

                QueryColumnDefinitionModel colDefinitionObj = new QueryColumnDefinitionModel();
                colDefinitionObj.Name = colHeader.ColumnName;
                colDefinitionObj.DataType = dbCommand.DbContext.DbDataType.GetDataType(colHeader.DataType.GetType());
                if (colHeader.IsNotNull) colDefinitionObj.ConstraintNotNull = dbCommand.NotNullString;
                if (colHeader.IsUnique) colDefinitionObj.ConstraintUnique = dbCommand.UniqueString;

                string colDefinitionStringValue = dbCommand.ColumnDefinitionString.Inject(colDefinitionObj);

                // There might be empty data which creates multiple whitespaces. Hence it is important to Trim to make 
                // iterator look good. Although this is not needed for the query to qork as it is cosmetic
                colDefinitionStringValue.Trim();

                columnDefinitions.Add(colDefinitionStringValue);

                if (colHeader.IsPK) primaryKeys.Add(colHeader.ColumnName);
            }

            string primaryKeyStringValue =
                ((primaryKeys.Count > 0) ? string.Format(dbCommand.PrimaryKeyConstraintString, string.Join(",", primaryKeys)) : "");

            SQLs.Add(dbCommand.CreateTableString.Inject(
                new Dictionary<string, string>()
                {
                    { "TableName", dynTable.TableName },
                    { "ColDefs",  string.Join(",", columnDefinitions) },
                    { "PKs", primaryKeyStringValue }
                }));
        }

        #endregion Generate

        /// <summary>
        /// An internal data model class which stores a column definition line information in Insert command
        /// </summary>
        public class QueryColumnDefinitionModel
        {
            /// <summary>
            /// The column name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The name of the data type (Database specific)
            /// </summary>
            public string DataType { get; set; }

            /// <summary>
            /// Constraint not null. Empty if not present
            /// </summary>
            public string ConstraintNotNull { get; set; }

            /// <summary>
            /// Constraint unique. Empty if not present
            /// </summary>
            public string ConstraintUnique { get; set; }
        }
    }
}
