using ScrapEngine.Common;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// The element of the scrap html table xml config
    /// </summary>
    public class ScrapHtmlTableElement : ScrapElement
    {
        #region Xml Attributes

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.XPathAttributeName, IsMandatory = true)]
        public string XPath { get; set; }

        #endregion Xml Attributes
    }
}
