using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class BaseAttribute : Attribute
    {
        public string Trim { get; set; }

        public BaseAttribute(string trim = " ")
        {
            Trim = trim;
        }
    }
}
