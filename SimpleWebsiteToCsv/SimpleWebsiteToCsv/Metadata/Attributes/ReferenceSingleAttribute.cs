using System;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class ReferenceSingleAttribute : QSXPathAttribute
    {
        
        public string Attribute { get; set; }
        public bool UseInnerHtml { get; set; }
        

        public ReferenceSingleAttribute(string xpath = "") : base(xpath)
        {
        }

        public ReferenceSingleAttribute(string xpathformat, params string[] args)
        {
            XPathFormat = xpathformat;
            XPathFormatArgs = args;
        }
    }
}
