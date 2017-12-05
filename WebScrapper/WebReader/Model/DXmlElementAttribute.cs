using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReader.Model
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class DXmlElementAttribute : Attribute
    {
        /// <summary>
        /// The name of the xml attribute
        /// </summary>
        public string Name { get; set; }

        public Type DerivedType { get; set; }

        /// <summary>
        /// Sets a check if the attribute is mandatory
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Constructor with Name parameter
        /// </summary>
        /// <param name="name"></param>
        public DXmlElementAttribute(string name, Type derivedType = null)
        {
            Name = name;
            DerivedType = derivedType;
            IsMandatory = false;
        }
    }
}
