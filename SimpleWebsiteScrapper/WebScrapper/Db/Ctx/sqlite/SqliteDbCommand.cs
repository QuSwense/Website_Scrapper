using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WebScrapper.Db.Config;
using WebScrapper.Db.Ctx.Db;

namespace WebScrapper.Db.Ctx
{
    public class SqliteDbCommand : DynamicDbCommand<
        DTable,
        DynamicRow,
        DColumnMetadata,
        SQLiteConnection,
        ColumnDbConfig,
        DColumn
        >
    {
        public SqliteDbCommand() { }
        public SqliteDbCommand(IDbContext dbContext, SQLiteConnection connectionCtx) 
            : base(dbContext, connectionCtx) { }

        public override void ExecuteDDL()
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                SQLiteCommand command = new SQLiteCommand(SQL, (SQLiteConnection)ConnectionCtx);
                command.ExecuteNonQuery();
            }
        }

        public override DbDataReader ExecuteDML()
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                using (SQLiteCommand fmd = (SQLiteCommand)ConnectionCtx.CreateCommand())
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
