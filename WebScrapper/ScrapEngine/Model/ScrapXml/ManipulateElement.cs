using ScrapEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// Refers to the Manipulate node of xml config
    /// </summary>
    [DXmlElement(ScrapXmlConsts.ManipulateNodeName)]
    public class ManipulateElement
    {
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
        public List<ManipulateChildElement> Manipulations { get; set; }
    }
}
