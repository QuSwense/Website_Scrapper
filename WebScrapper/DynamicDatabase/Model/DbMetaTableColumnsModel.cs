using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Db.Meta;

namespace DynamicDatabase.Model
{
    [DDTable("mtblcol")]
    public class DbMetaTableColumnsModel
    {
        [DDPrimaryKey]
        [DDColumn("tnm")]
        public string TableName { get; set; }

        [DDPrimaryKey]
        [DDColumn("col")]
        public string ColumnName { get; set; }

        [DDColumn("desc")]
        public string Description { get; set; }

        [DDColumn("url")]
        public string Url { get; set; }

        [DDColumn("xpath")]
        public string XPath { get; set; }
    }
}
