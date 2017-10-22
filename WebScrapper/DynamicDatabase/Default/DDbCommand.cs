using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WebScrapper.Db.Config;

namespace DynamicDatabase.Default
{
    public class DDbCommand : DynamicDbCommand<
        DTable,
        DynamicRow,
        DColumnMetadata,
        SQLiteConnection,
        ColumnDbConfig,
        DColumn
        >
    {
        public DDbCommand()
        {
        }

        public DDbCommand(IDbContext dbContext, SQLiteConnection connectionCtx) : base(dbContext, connectionCtx)
        {
        }
    }
}
