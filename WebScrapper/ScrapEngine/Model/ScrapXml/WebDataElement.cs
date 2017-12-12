using ScrapEngine.Common;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The root element of a Xml configuration file for scrapping web data from a website or
    /// multiple websites.
    /// </summary>
    [DXmlElement(ScrapXmlConsts.WebDataNodeName)]
    public class WebDataElement
    {
        #region References

        /// <summary>
        /// A Root node may contain different type of Scrap class elements which has same base type
        /// <see cref="ScrapElement"/>
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ScrapCsvNodeName, DerivedType = typeof(ScrapCsvElement))]
        [DXmlElement(ScrapXmlConsts.ScrapHtmlTableNodeName, DerivedType = typeof(ScrapHtmlTableElement))]
        public List<ScrapElement> Scraps { get; set; }

        #endregion References
    }
}
