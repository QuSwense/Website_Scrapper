using System;

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
        /// Sets a check if the attribute is mandatory
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// The default value in case the attribute is not defined
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        /// Constructor with Name parameter
        /// </summary>
        /// <param name="name"></param>
        public DXmlAttributeAttribute(string name, object defVal = null)
        {
            Name = name;
            Default = defVal;
            IsMandatory = false;
        }
    }
}
