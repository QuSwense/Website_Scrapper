using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    public class ManipulateChildFactory
    {
        /// <summary>
        /// Split Manipulate tag
        /// </summary>
        private ScrapSplitConfigParser scrapSplitConfigParser;

        /// <summary>
        /// Trim Manipulate tag
        /// </summary>
        private ScrapTrimConfigParser scrapTrimConfigParser;

        /// <summary>
        /// Replace Manipulate tag
        /// </summary>
        private ScrapReplaceConfigParser scrapReplaceConfigParser;

        /// <summary>
        /// Regex Manipulate tag
        /// </summary>
        private ScrapRegexConfigParser scrapRegexConfigParser;

        private ScrapRegexReplaceParser scrapRegexReplaceParser;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ManipulateChildFactory(WebScrapConfigParser configParser)
        {
            scrapSplitConfigParser = new ScrapSplitConfigParser(configParser);
            scrapTrimConfigParser = new ScrapTrimConfigParser(configParser);
            scrapReplaceConfigParser = new ScrapReplaceConfigParser(configParser);
            scrapRegexConfigParser = new ScrapRegexConfigParser(configParser);
            scrapRegexReplaceParser = new ScrapRegexReplaceParser(configParser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mcelement"></param>
        /// <returns></returns>
        public ScrapManipulateChildConfigParser GetParser(ManipulateChildElement manipulateChild)
        {
            if (manipulateChild is SplitElement)
                return scrapSplitConfigParser;
            else if (manipulateChild is TrimElement)
                return scrapTrimConfigParser;
            else if (manipulateChild is ReplaceElement)
                return scrapReplaceConfigParser;
            else if (manipulateChild is RegexElement)
                return scrapRegexConfigParser;
            else if (manipulateChild is RegexReplaceElement)
                return scrapRegexReplaceParser;
            else
                throw new ScrapParserException(ScrapParserException.EErrorType.UNKNOWN_MANIPULATE_CHILD_TYPE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ScrapManipulateChildConfigParser GetParser(string name)
        {
            if (name == "Split") return scrapSplitConfigParser;
            else if (name == "Trim") return scrapTrimConfigParser;
            else if (name == "Regex") return scrapRegexConfigParser;
            else if (name == "Replace") return scrapReplaceConfigParser;
            else if (name == "RegexReplace") return scrapRegexReplaceParser;
            else
                throw new ScrapXmlException(ScrapXmlException.EErrorType.INVALID_MANIPULATE_CHILD_ITEM);
        }

        /// <summary>
        /// Create an instance of Manipulate child
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ManipulateChildElement Create(string name)
        {
            if (name == "Split") return new SplitElement();
            else if (name == "Trim") return new TrimElement();
            else if (name == "Regex") return new RegexElement();
            else if (name == "Replace") return new ReplaceElement();
            else if (name == "RegexReplace") return new RegexReplaceElement();
            else
                throw new ScrapXmlException(ScrapXmlException.EErrorType.INVALID_MANIPULATE_CHILD_ITEM);
        }
    }
}
