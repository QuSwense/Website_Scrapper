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

        /// <summary>
        /// Regex Replace Manipulate parser
        /// </summary>
        private ScrapRegexReplaceParser scrapRegexReplaceParser;

        /// <summary>
        /// Regex Replace Manipulate parser
        /// </summary>
        private ScrapPurgeConfigParser scrapPurgeConfigParser;

        private ScrapDbChangeConfigParser scrapDbChangeConfigParser;
        private ScrapHtmlDecodeConfigParser scrapHtmlDecodeConfigParser;

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
            scrapPurgeConfigParser = new ScrapPurgeConfigParser(configParser);
            scrapDbChangeConfigParser = new ScrapDbChangeConfigParser(configParser);
            scrapHtmlDecodeConfigParser = new ScrapHtmlDecodeConfigParser(configParser);
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
            else if (manipulateChild is PurgeElement)
                return scrapPurgeConfigParser;
            else if (manipulateChild is DbchangeElement)
                return scrapDbChangeConfigParser;
            else if (manipulateChild is HtmlDecodeElement)
                return scrapHtmlDecodeConfigParser;
            else
                throw new ScrapParserException(ScrapParserException.EErrorType.UNKNOWN_MANIPULATE_CHILD_TYPE);
        }
    }
}
