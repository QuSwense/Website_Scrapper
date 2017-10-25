using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Reader.Meta
{
    public class SplitIndexAttribute : Attribute
    {
        public int Index { get; set; }

        public SplitIndexAttribute(int index)
        {
            Index = index;
        }
    }
}
