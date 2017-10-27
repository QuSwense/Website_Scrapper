using DynamicDatabase.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Model
{
    [DDTable("mdt")]
    public class DbMetaTableModel
    {
        [DDPrimaryKey]
        [DDColumn("tbl")]
        public string TableName { get; set; }

        [DDColumn("desc")]
        public string Description { get; set; }

        [DDColumn("url")]
        public string Url { get; set; }

        [DDColumn("xpath")]
        public string XPath { get; set; }
    }
}
