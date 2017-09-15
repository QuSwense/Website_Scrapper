using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class CitationRegexAttribute : CitationAttribute
    {
        public string ClassName { get; set; }
        public string PropertyName { get; set; }
        public string[] FixedValues { get; set; }
    }
}
