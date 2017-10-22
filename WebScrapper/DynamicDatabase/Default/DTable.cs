using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using WebScrapper.Db.Config;

namespace DynamicDatabase.Default
{
    public class DTable : DynamicTable<
        DynamicRow, 
        DColumnMetadata,
        ColumnDbConfig,
        DColumn>
    {
    }
}
