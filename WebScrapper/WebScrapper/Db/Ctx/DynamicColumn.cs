using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace WebScrapper.Db.Ctx
{
    public class DynamicColumn<TDynColMetadata>
        where TDynColMetadata : DynamicColumnMetadata, new()
    {
        public string ColumnName { get; protected set; }
        public TDynColMetadata Metadata { get; protected set; }
    }
}
