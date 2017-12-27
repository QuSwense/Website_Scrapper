using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// Refers to the Manipulate node of xml config
    /// </summary>
    [DXmlElement(ScrapXmlConsts.ManipulateNodeName)]
    public class ManipulateElement : ConfigElementBase
    {
        #region IConfigElement Implementation

        /// <summary>
        /// A set of child elements in order of occurance
        /// </summary>
        [DXmlElement(ScrapXmlConsts.DbchangeNodeName)]
        [DXmlElement(ScrapXmlConsts.HtmlDecodeNodeName)]
        [DXmlElement(ScrapXmlConsts.PurgeNodeName)]
        [DXmlElement(ScrapXmlConsts.RegexNodeName)]
        [DXmlElement(ScrapXmlConsts.RegexReplaceNodeName)]
        [DXmlElement(ScrapXmlConsts.ReplaceNodeName)]
        [DXmlElement(ScrapXmlConsts.SplitNodeName)]
        [DXmlElement(ScrapXmlConsts.TrimNodeName)]
        [DXmlElement(ScrapXmlConsts.ValidateNodeName)]
        public override List<IConfigElement> Children { get; set; }

        #endregion IConfigElement Implementation
    }
}
