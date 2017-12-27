using ScrapEngine.Common;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// This manipulate node uses "Regex.Replace"
    /// </summary>
    public class RegexReplaceElement : ConfigElementBase
    {
        #region Xml Attributes

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.PatternAttributeName, IsMandatory = true)]
        [DXmlNormalize]
        public string Pattern { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.ReplaceAttributeName)]
        [DXmlNormalize]
        public string Replace { get; set; }

        #endregion Xml Attributes
    }
}
