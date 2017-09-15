using System;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class ReferenceCollectionAttribute : QSXPathAttribute
    {
        public ReferenceCollectionAttribute(string xpath = "") : base(xpath)
        {
        }
    }
}
