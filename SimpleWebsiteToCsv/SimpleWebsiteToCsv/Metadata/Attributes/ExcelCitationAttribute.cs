using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class ExcelCitationAttribute : CitationAttribute
    {
        public string SheetName { get; set; }
        public int StartRowIndex { get; set; }
        public ExcelCitationAttribute(string sheetName = "", int row = 0)
        {
            SheetName = sheetName;
            StartRowIndex = row;
        }
    }
}
