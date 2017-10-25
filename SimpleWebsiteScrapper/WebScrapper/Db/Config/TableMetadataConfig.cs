using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Reader.Meta;

namespace WebScrapper.Db.Config
{
    public class TableMetadataConfigModel
    {
        [SplitIndex(1)]
        public string Display { get; set; }

        [SplitIndex(2)]
        public string Reference { get; set; }
    }
}
