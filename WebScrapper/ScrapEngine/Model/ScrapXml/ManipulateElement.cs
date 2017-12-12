using ScrapEngine.Common;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// Refers to the Manipulate node of xml config
    /// </summary>
    [DXmlElement(ScrapXmlConsts.ManipulateNodeName)]
    public class ManipulateElement
    {
        #region References

        /// <summary>
        /// Child manipulation nodes
        /// </summary>
        [DXmlElement(ScrapXmlConsts.DbchangeNodeName, typeof(DbchangeElement))]
        [DXmlElement(ScrapXmlConsts.HtmlDecodeNodeName, typeof(HtmlDecodeElement))]
        [DXmlElement(ScrapXmlConsts.PurgeNodeName, typeof(PurgeElement))]
        [DXmlElement(ScrapXmlConsts.RegexNodeName, typeof(RegexElement))]
        [DXmlElement(ScrapXmlConsts.RegexReplaceNodeName, typeof(RegexReplaceElement))]
        [DXmlElement(ScrapXmlConsts.ReplaceNodeName, typeof(ReplaceElement))]
        [DXmlElement(ScrapXmlConsts.SplitNodeName, typeof(SplitElement))]
        [DXmlElement(ScrapXmlConsts.TrimNodeName, typeof(TrimElement))]
        public List<ManipulateChildElement> ManipulateChilds { get; set; }

        /// <summary>
        /// Points to the parent scrap node
        /// </summary>
        [DXmlParent]
        public ColumnElement Parent { get; set; }

        #endregion References
    }
}
