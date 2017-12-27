using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// The Manipulate child node will manipulate data and intercation with database to 
    /// modify the data
    /// </summary>
    public class DbchangeElement : ManipulateChildElement
    {
        #region Xml Attributes

        /// <summary>
        /// The Existence query clase
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsExistsAttributeName, IsMandatory = true)]
        public string IsExistsQuery { get; set; }

        #endregion Xml Attributes

        #region IConfigElement Implementation

        /// <summary>
        /// A set of child elements in order of occurance
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ManipulateNodeName)]
        public override List<IConfigElement> Children { get; set; }

        #endregion IConfigElement Implementation

        #region References

        /// <summary>
        /// The Exists node which checks the existence of node
        /// </summary>
        [DXmlElement(ScrapXmlConsts.DbchangeSelectNodeName)]
        public IConfigElement Select { get; set; }

        #endregion References
    }
}
