using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata
{
    public class UniqueDictionaryKeyAttribute : Attribute
    {
        public bool IsDefault { get; set; }

        public UniqueDictionaryKeyAttribute(bool isDefault = false)
        {
            IsDefault = isDefault;
        }
    }
}
