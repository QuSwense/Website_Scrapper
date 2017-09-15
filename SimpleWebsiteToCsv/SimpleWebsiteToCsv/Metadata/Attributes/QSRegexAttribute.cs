using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class QSRegexAttribute : Attribute
    {
        public string Pattern { get; set; }

        public QSRegexAttribute(string pattern = "")
        {
            Pattern = pattern;
        }
    }
}
