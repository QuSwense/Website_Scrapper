using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebReader.Model
{
    /// <summary>
    /// The attribute which can be used in a class along with a Property to 
    /// represent an Xml Attribute
    /// </summary>
    public class DXmlAttributeAttribute : Attribute
    {
        /// <summary>
        /// The name of the xml attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor with Name parameter
        /// </summary>
        /// <param name="name"></param>
        public DXmlAttributeAttribute(string name)
        {
            Name = name;
        }
    }
}
