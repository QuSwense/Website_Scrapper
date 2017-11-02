using DynamicDatabase;
using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Sqlite
{
    public class SqliteDbCommand : DynamicDbCommand
    {
        public SqliteDbCommand() { }

        public override void ExecuteDDL()
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                SQLiteCommand command = new SQLiteCommand(SQL, (SQLiteConnection)(dbContext.DbConnection.Connection));
                command.ExecuteNonQuery();
            }
        }

        public override DbDataReader ExecuteDML()
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                using (SQLiteCommand fmd = ((SQLiteCommand)dbContext.DbConnection.Connection.CreateCommand()))
                {
                    fmd.CommandText = SQL;
                    fmd.CommandType = CommandType.Text;
                    return fmd.ExecuteReader();
                }
            }

            return null;
        }

        public override DbDataReader LoadMetadata(string name)
        {
            SQL = string.Format("PRAGMA table_info('{0}')", name);
            return ExecuteDML();
        }
    }
}
