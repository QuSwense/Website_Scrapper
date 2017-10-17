using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Db.Meta;

namespace WebScrapper.Db
{
    [DDTable("tblmdt")]
    public class TableMetadataModel
    {
        [DDPrimaryKey]
        [DDColumn("tnm")]
        public string TableName { get; set; }

        [DDColumn("desc")]
        public string Description { get; set; }
    }
}
