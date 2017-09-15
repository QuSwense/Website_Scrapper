using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class QSURIFileDataSourceAttribute : QSURIDataSourceAttribute
    {
        public string XPath { get; set; }
    }
}
