using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class QSURIExcelDataSourceAttribute : QSURIDataSourceAttribute
    {
        public string SheetName { get; set; }
    }
}
