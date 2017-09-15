using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class FileIteratorAttribute : Attribute
    {
        public int StartIndex { get; set; }
        public string StartFromContentRegex { get; set; }
        public string StopText { get; set; }

        public bool UseNewLine { get; set; }

        public FileIteratorAttribute()
        {
            UseNewLine = true;
        }
    }
}
