using ScrapEngine.Common;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The root element of the scrap xml config
    /// </summary>
    public class ScrapHtmlTableElement : ScrapElement
    {
        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.XPathAttributeName, IsMandatory = true)]
        public string XPath { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapHtmlTableElement() { }
    }
}
