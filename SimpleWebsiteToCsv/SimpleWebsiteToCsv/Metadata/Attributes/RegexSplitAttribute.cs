using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class RegexSplitAttribute : Attribute
    {
        public string Pattern { get; set; }

        public RegexSplitAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
