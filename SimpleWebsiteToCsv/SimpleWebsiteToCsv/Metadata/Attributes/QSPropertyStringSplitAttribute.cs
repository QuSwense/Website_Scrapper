using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class QSPropertyStringSplitAttribute : StringSplitAttribute
    {
        public string PropertyName { get; set; }

        public QSPropertyStringSplitAttribute(string split) : base(split)
        {
        }
    }
}
