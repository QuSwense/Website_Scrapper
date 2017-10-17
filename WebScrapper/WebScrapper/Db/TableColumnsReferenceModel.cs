using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Db.Meta;

namespace WebScrapper.Db
{
    [DDTable("tblcolsref")]
    public class TableColumnsReferenceModel
    {
        [DDPrimaryKey]
        [DDColumn("tnm")]
        public string TableName { get; set; }

        [DDPrimaryKey]
        [DDColumn("col")]
        public string ColumnName { get; set; }

        [DDPrimaryKey]
        [DDColumn("irow")]
        public int RowIndex { get; set; }

        [DDColumn("rurl")]
        public string ReferenceUrl { get; set; }

        [DDColumn("rxpath")]
        public string ReferenceXPath { get; set; }

        [DDColumn("thref")]
        public string TargetHref { get; set; }
    }
}
