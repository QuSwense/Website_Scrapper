using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    public class QSIndexAttribute : Attribute
    {
        public int Index { get; set; }

        /// <summary>
        /// Only used with conjugation of a QSRegex Attribute on class with 'name' group option
        /// </summary>
        public string GroupName { get; set; }

        public QSIndexAttribute(int index = -1)
        {
            Index = index;
        }
    }
}
