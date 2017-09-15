using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class QSRegexNamedSplitAttribute : QSRegexAttribute
    {
        public QSRegexNamedSplitAttribute(string pattern = "") : base(pattern) { }
    }
}
