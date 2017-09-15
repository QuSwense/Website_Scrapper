using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class CSVCitationAttribute : CitationAttribute
    {
        public int StartRow { get; set; }
        public string Seperator { get; set; }

        public CSVCitationAttribute()
        {
            Seperator = ",";
        }
    }
}
