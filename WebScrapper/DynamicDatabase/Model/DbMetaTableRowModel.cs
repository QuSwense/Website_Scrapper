using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebScrapper.Db.Meta;

namespace DynamicDatabase.Model
{
    [DDTable("mtblrow")]
    public class DbMetaTableRowModel
    {
        [DDPrimaryKey]
        [DDColumn("tnm")]
        public string TableName { get; set; }

        [DDPrimaryKey]
        [DDColumn("col")]
        public string ColumnName { get; set; }

        [DDPrimaryKey]
        [DDColumn("rid")]
        public string RowId { get; set; }

        [DDColumn("xpath")]
        public string XPath { get; set; }
    }
}
