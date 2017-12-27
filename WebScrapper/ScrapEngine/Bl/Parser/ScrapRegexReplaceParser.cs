using ScrapEngine.Model;
using ScrapEngine.Model.Scrap;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapRegexReplaceParser : ScrapManipulateChildConfigParser
    {
        public ScrapRegexReplaceParser() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapRegexReplaceParser(WebScrapConfigParser configParser)
            : base(configParser) { }
        
        /// <summary>
        /// Process multiple results
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData manipulateHtml, ManipulateChildElement manipulateChild)
        {
            if (string.IsNullOrEmpty(manipulateHtml.OriginalValue)) return;

            RegexReplaceElement regexConfig = (RegexReplaceElement)manipulateChild;

            List<string> finalResults = new List<string>();
            Regex regexPattern = new Regex(regexConfig.Pattern);

            for (int i = 0; i < manipulateHtml.Results.Count; ++i)
            {
                finalResults.Add(Regex.Replace(manipulateHtml.Results[i], regexConfig.Pattern, regexConfig.Replace));
            }

            manipulateHtml.Results = new List<string>(finalResults);
        }

        /// <summary>
        /// Assert regex element
        /// </summary>
        /// <param name="regexElement"></param>
        private void Assert(RegexReplaceElement regexElement)
        {
            if (string.IsNullOrEmpty(regexElement.Pattern))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_DATA_EMPTY);
        }
    }
}
