using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Web
{
    public class TableDataColumnModel
    {
        public string Name { get; set; }
        public bool IsPk { get; set; }
        public string Value { get; set; }
        public int RowIndex { get; set; }
    }
}
