using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Parser
{
    public class ClassParseState : ParseState
    {
        public List<QSReferenceAttribute> References { get; set; }
        public CitationAttribute Citation { get; set; }
    }
}
