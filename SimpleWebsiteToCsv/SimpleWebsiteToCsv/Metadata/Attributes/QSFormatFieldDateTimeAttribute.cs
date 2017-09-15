using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class QSFormatFieldDateTimeAttribute : Attribute
    {
        public string Format { get; set; }

        public QSFormatFieldDateTimeAttribute(string format)
        {
            Format = format;
        }
    }
}
