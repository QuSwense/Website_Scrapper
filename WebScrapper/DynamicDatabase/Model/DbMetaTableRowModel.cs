using DynamicDatabase.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Model
{
    [DDTable("mdtrow")]
    public class DbMetaTableRowModel
    {
        [DDPrimaryKey]
        [DDColumn("col")]
        public string ColumnName { get; set; }

        [DDPrimaryKey]
        [DDColumn("pk")]
        public string PrimaryKey { get; set; }

        [DDColumn("url")]
        public string Url { get; set; }

        [DDColumn("xpath")]
        public string XPath { get; set; }

        [DDPrimaryKey]
        [DDColumn("desc")]
        public string Description { get; set; }
    }
}
