using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class QSReferenceAttribute : Attribute
    {
        public string Url { get; set; }
        public string Text { get; set; }

        public QSReferenceAttribute(string url = "", string text = "")
        {
            Url = url;
            Text = text;
        }
    }
}
