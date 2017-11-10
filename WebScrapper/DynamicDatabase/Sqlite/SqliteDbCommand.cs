using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace DynamicDatabase.Sqlite
{
    /// <summary>
    /// A Command class for Database query execution which is specific to SQLITE databases
    /// </summary>
    public class SqliteDbCommand : DynamicDbCommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SqliteDbCommand() { }

        #endregion Constructor

        #region Create

        /// <summary>
        /// Remove the table
        /// </summary>
        /// <param name="tableName"></param>
        public override void RemoveTable(string tableName)
        {
            SetSQL(string.Format("DROP TABLE [IF EXISTS] {0};", tableName));
            ExecuteDDL();
        }

        #endregion Create

        #region Load

        /// <summary>
        /// Load the table metadata information
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override DbDataReader LoadTableMetadata(string name)
        {
            SetSQL(string.Format(TableInformationString, name));
            return ExecuteDML();
        }

        #endregion Load

        #region Execute

        /// <summary>
        /// Execute Data Definiton Language
        /// </summary>
        public override void ExecuteDDL()
        {
            foreach (var sqlQuery in SQLs)
            {
                if (!string.IsNullOrEmpty(sqlQuery))
                {
                    SQLiteCommand command = new SQLiteCommand(sqlQuery, (SQLiteConnection)(DbContext.DbConnection.Connection));
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Execute the Data manipulation query and return the reader
        /// </summary>
        /// <returns></returns>
        public override DbDataReader ExecuteDML()
        {
            foreach (var sqlQuery in SQLs)
            {
                if (!string.IsNullOrEmpty(sqlQuery))
                {
                    using (SQLiteCommand fmd = ((SQLiteCommand)DbContext.DbConnection.Connection.CreateCommand()))
                    {
                        fmd.CommandText = sqlQuery;
                        fmd.CommandType = CommandType.Text;
                        return fmd.ExecuteReader();
                    }
                }
            }

            return null;
        }

        #endregion Execute

        #region Helper
        
        /// <summary>
        /// Gets the Insert query format for the database
        /// </summary>
        /// <returns></returns>
        public override string InsertOrReplaceQueryString
        { get { return "INSERT OR REPLACE INTO {TableName} VALUES ( {Values} )"; } }

        #endregion Helper

        #region Format

        /// <summary>
        /// Gets the information / metdata about a table from the database
        /// </summary>
        public override string TableInformationString { get { return "PRAGMA table_info('{0}')"; } }

        /// <summary>
        /// Gets the existence of a table from the database
        /// </summary>
        public override string TableExistenceString
        { get { return "SELECT DISTINCT tbl_name from sqlite_master where tbl_name = '{0}'"; } }

        #endregion Format
    }
}
