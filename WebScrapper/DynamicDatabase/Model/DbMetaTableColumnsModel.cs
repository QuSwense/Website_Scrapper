using DynamicDatabase.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDatabase.Model
{
    [DDTable("mdtcol")]
    public class DbMetaTableColumnsModel
    {
        [DDPrimaryKey]
        [DDColumn("col")]
        public string ColumnName { get; set; }

        [DDPrimaryKey]
        [DDColumn("dspl")]
        public string Display { get; set; }

        [DDColumn("desc")]
        public string Description { get; set; }

        [DDColumn("url")]
        public string Url { get; set; }

        [DDColumn("xpath")]
        public string XPath { get; set; }
    }
}
