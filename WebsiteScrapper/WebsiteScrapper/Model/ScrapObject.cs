using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteScrapper.Model
{
    public class ScrapObject<T>
    {
        public string Description { get; set; }
        public ScrapString Reference { get; set; }
        public ScrapString Copyright { get; set; }
        public string Url { get; set; }
        public string XPath { get; set; }
        public string Attribute { get; set; }
        public T Value { get; set; }
    }
}
