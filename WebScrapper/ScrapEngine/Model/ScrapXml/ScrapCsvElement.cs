using ScrapEngine.Common;
using WebCommon.Const;
using WebReader.Model;

namespace ScrapEngine.Model
{
    public class ScrapCsvElement : ScrapElement
    {
        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.DelimiterAttributeName, IsMandatory = true)]
        public string Delimiter { get; set; }

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.SkipFirstAttributeName)]
        public int SkipFirstLines { get; set; }

        public ScrapCsvElement()
        {
            SkipFirstLines = 0;
            Delimiter = ASCIICharacters.Tab;
        }
    }
}
