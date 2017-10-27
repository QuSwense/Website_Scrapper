using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Model
{
    public class TableDataColumnModel
    {
        public string Name { get; set; }
        public bool IsPk { get; set; }
        public string Value { get; set; }
        public int RowIndex { get; set; }
        public string Url { get; set; }
        public string XPath { get; set; }
    }
}
