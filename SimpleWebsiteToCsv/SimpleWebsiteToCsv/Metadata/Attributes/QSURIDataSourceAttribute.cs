using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class QSURIDataSourceAttribute : Attribute
    {
        public string Online { get; set; }
        public string Offline { get; set; }
    }
}
