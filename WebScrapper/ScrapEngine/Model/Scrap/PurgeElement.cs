using ScrapEngine.Common;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// Manipulate child element which purges data on condition
    /// </summary>
    public class PurgeElement : ConfigElementBase
    {
        #region Xml Attributes

        /// <summary>
        /// It is a check that if the data is empty or null then remove it from to be used
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsEmptyOrNullAttributeName,
            ConfigElementConsts.IsEmptyOrNullColumnDefault)]
        public bool IsEmptyOrNull { get; set; }

        /// <summary>
        /// It is a check that if the data is empty or null then remove it from to be used.
        /// If it is true then remove if whitespace or null
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsWhitespaceAttributeName,
            ConfigElementConsts.IsWhitespaceColumnDefault)]
        public bool IsWhitespace { get; set; }

        #endregion Xml Attributes
    }
}
