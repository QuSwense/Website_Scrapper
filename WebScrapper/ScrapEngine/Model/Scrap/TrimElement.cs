using ScrapEngine.Common;
using ScrapEngine.Model.Scrap;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// Manipulate node data
    /// </summary>
    public class TrimElement : ConfigElementBase
    {
        #region Xml Attributes

        /// <summary>
        /// The data
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.DataAttributeName)]
        [DXmlNormalize]
        public string Data { get; set; }

        #endregion Xml Attributes
    }
}
