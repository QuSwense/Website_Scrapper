using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using ScrapEngine.Model.Scrap;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The root element of a Xml configuration file for scrapping web data from a website or
    /// multiple websites.
    /// </summary>
    [DXmlElement(ScrapXmlConsts.WebDataNodeName)]
    public class WebDataElement : ConfigElementBase
    {
        #region IConfigElement Implementation

        /// <summary>
        /// A Root node may contain different type of Scrap class elements which has same base type
        /// <see cref="ScrapElement"/>
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ScrapCsvNodeName, DerivedType = typeof(ScrapCsvElement))]
        [DXmlElement(ScrapXmlConsts.ScrapHtmlTableNodeName, DerivedType = typeof(ScrapHtmlTableElement))]
        public override List<IConfigElement> Children { get; set; }

        #endregion IConfigElement Implementation
    }
}
