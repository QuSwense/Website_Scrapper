using SimpleWebsiteToCsv.Metadata.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class AboutAttribute : Attribute
    {
        public string Description { get; set; }
        public ReferenceUriField Reference { get; set; }

        public AboutAttribute(string description = "")
        {
            Description = description;
        }
    }
}
