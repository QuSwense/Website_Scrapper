using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Web
{
    public class ColumnScrapModel
    {
        public bool IsPk { get; set; }
        public string Value { get; set; }
        public string ActualColumn { get; set; }
        public string ActualTable { get; set; }
    }
}
