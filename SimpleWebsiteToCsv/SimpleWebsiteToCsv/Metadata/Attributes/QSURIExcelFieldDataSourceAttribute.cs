using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QSURIExcelFieldDataSourceAttribute : QSReferenceAttribute
    {
        public string SheetName { get; set; }

        public int StartRowIndex { get; set; }
    }
}
