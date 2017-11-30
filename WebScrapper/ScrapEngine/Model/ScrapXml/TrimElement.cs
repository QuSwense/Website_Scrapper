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
        /// <summary>
        /// The data
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.DataAttributeName)]
        public string Data { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TrimElement() { }
    }
}
