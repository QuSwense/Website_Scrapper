using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class BoolConversionAttribute : Attribute
    {
        /// <summary>
        /// Comma delimited truth values like, Yes, true, etc.
        /// </summary>
        public string TruthValues { get; set; }
    }
}
