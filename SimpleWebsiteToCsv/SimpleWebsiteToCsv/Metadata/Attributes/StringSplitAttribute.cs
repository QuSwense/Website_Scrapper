using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class StringSplitAttribute : Attribute
    {
        public string Split { get; set; }

        public StringSplitAttribute(string split = ",")
        {
            Split = split;
        }
    }
}
