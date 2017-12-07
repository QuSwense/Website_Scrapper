using ScrapEngine.Common;
using ScrapEngine.Model.ScrapXml;
using WebReader.Model;

namespace ScrapEngine.Model
{
    public class ReplaceElement : ManipulateChildElement
    {
        #region Xml Attributes

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.InAttributeName, IsMandatory = true)]
        public string InString { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.OutAttributeName, IsMandatory = true)]
        public string OutString { get; set; }

        #endregion Xml Attributes
    }
}
