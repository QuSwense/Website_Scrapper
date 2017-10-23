using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Default
{
    public class DDbCommand : DynamicDbCommand<
        DTable,
        DynamicRow,
        DColumnMetadata,
        DbConnection
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
