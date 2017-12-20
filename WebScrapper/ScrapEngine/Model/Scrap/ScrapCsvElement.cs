using ScrapEngine.Common;
using WebCommon.Const;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// Used to fetch a csv/text file from html page and scrap the data
    /// </summary>
    public class ScrapCsvElement : ScrapElement
    {
        #region Xml Attributes

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.DelimiterAttributeName, IsMandatory = true)]
        [DXmlNormalize]
        public string Delimiter { get; set; }

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.SkipFirstAttributeName)]
        public int SkipFirstLines { get; set; }

        #endregion Xml Attributes

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScrapCsvElement()
        {
            SkipFirstLines = 0;
            Delimiter = ASCIICharacters.TabString;
        }

        #endregion Constructor
    }
}
