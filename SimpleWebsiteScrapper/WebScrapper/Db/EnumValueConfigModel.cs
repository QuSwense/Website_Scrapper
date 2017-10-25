using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Reader.Meta;

namespace WebScrapper.Db
{
    public class EnumValueConfigModel
    {
        [SplitIndex(1)]
        public int Value { get; set; }

        [SplitIndex(2)]
        public string Description { get; set; }
    }
}
