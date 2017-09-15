using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class QSUriRTTIDataSourceAttribute : Attribute
    {
        public string ClassPath { get; set; }
        public string Property { get; set; }
        public string Format { get; set; }

        public QSUriRTTIDataSourceAttribute(string classPath = "", string property = "")
        {
            ClassPath = classPath;
            Property = property;
        }
    }
}
