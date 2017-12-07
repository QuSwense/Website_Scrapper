using ScrapEngine.Common;
using ScrapEngine.Model.ScrapXml;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// Manipulate node data
    /// </summary>
    public class TrimElement : ManipulateChildElement
    {
        #region Xml Attributes

        /// <summary>
        /// The data
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.DataAttributeName)]
        public string Data { get; set; }

        #endregion Xml Attributes
    }
}
