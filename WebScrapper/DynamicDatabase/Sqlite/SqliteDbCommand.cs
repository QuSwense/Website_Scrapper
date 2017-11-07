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
            SQL = string.Format("DROP TABLE [IF EXISTS] {0};", tableName);
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
            SQL = string.Format("PRAGMA table_info('{0}')", name);
            return ExecuteDML();
        }

        #endregion Load

        #region Execute

        /// <summary>
        /// Execute Data Definiton Language
        /// </summary>
        public override void ExecuteDDL()
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                SQLiteCommand command = new SQLiteCommand(SQL, (SQLiteConnection)(DbContext.DbConnection.Connection));
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Execute the Data manipulation query and return the reader
        /// </summary>
        /// <returns></returns>
        public override DbDataReader ExecuteDML()
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                using (SQLiteCommand fmd = ((SQLiteCommand)DbContext.DbConnection.Connection.CreateCommand()))
                {
                    fmd.CommandText = SQL;
                    fmd.CommandType = CommandType.Text;
                    return fmd.ExecuteReader();
                }
            }

            return null;
        }

        #endregion Execute
    }
}
