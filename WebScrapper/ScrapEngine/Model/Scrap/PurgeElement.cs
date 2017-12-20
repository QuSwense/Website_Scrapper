using ScrapEngine.Common;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// Manipulate child element which purges data on condition
    /// </summary>
    public class PurgeElement : ManipulateChildElement
    {
        #region Xml Attributes

        /// <summary>
        /// It is a check that if the data is empty or null then remove it from to be used
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsEmptyOrNullAttributeName)]
        public bool IsEmptyOrNull { get; set; }

        /// <summary>
        /// It is a check that if the data is empty or null then remove it from to be used
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsWhitespaceAttributeName)]
        public bool IsWhitespace { get; set; }

        #endregion Xml Attributes

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PurgeElement()
        {
            IsEmptyOrNull = false;
            IsWhitespace = false;
        }

        #endregion Constructor
    }
}
