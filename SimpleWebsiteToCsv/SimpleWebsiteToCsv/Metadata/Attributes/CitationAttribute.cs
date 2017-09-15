using SimpleWebsiteToCsv.Metadata.Field;
using System;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class CitationAttribute : Attribute
    {
        public string Copyright { get; set; }
        public string CitationString { get; set; }
        public string LogoOnlineUrl { get; set; }
    }
}
