using ScrapEngine.Model;
using ScrapEngine.Model.Scrap;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The business logic class which parses a Regex tag and interprets the results
    /// </summary>
    public class ScrapRegexConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapRegexConfigParser() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapRegexConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }
        
        /// <summary>
        /// Process multiple results
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            if (string.IsNullOrEmpty(result.OriginalValue)) return;

            RegexElement regexConfig = (RegexElement)manipulateChild;

            List<string> finalResults = new List<string>();
            Regex regexPattern = new Regex(regexConfig.Pattern);

            for(int i = 0; i < result.Results.Count; ++i)
            {
                MatchCollection matchlist = regexPattern.Matches(result.Results[i]);

                if (regexConfig.Index >= 0 && matchlist.Count > regexConfig.Index)
                        finalResults.Add(matchlist[regexConfig.Index].Value);
                else if (regexConfig.Index == -1)
                    foreach (Match match in matchlist)
                        finalResults.Add(match.Value);
                else if (regexConfig.Index == -2 && matchlist.Count > 0) // Add last
                    finalResults.Add(matchlist[matchlist.Count - 1].Value);
                else finalResults.Add(result.Results[i]);
            }

            result.Results = new List<string>(finalResults);
        }

        /// <summary>
        /// Assert regex element
        /// </summary>
        /// <param name="regexElement"></param>
        private void Assert(RegexElement regexElement)
        {
            if (string.IsNullOrEmpty(regexElement.Pattern))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_DATA_EMPTY);
            else if (!(regexElement.Index == -1 || regexElement.Index == -2 || regexElement.Index >= 0))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_INDEX_INVALID,
                    new string[] { regexElement.Index.ToString() });
        }
    }
}
