using System;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class RegexReplaceAttribute : Attribute
    {
        public int Order { get; set; }
        public string Pattern { get; set; }
        public string ReplaceText { get; set; }

        public RegexReplaceAttribute()
        {
            Order = Int32.MinValue;
            ReplaceText = "";
        }
    }
}
