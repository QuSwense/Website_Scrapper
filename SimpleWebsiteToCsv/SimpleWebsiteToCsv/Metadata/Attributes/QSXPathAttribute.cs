using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class QSXPathAttribute : BaseAttribute
    {
        public string XPath { get; set; }
        public string XPathFormat { get; set; }
        public string[] XPathFormatArgs { get; set; }

        public QSXPathAttribute(string xpath = "")
        {
            XPath = xpath;
        }

        public QSXPathAttribute(string xpathformat, params string[] args)
        {
            XPathFormat = xpathformat;
            XPathFormatArgs = args;
        }
    }
}
