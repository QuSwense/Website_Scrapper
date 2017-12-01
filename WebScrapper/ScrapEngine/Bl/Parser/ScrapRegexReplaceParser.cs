using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapRegexReplaceParser : ScrapManipulateChildConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapRegexReplaceParser(WebScrapConfigParser configParser)
            : base(configParser) { }
        
        /// <summary>
        /// Process multiple results
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            if (string.IsNullOrEmpty(result.OriginalValue)) return;

            RegexReplaceElement regexConfig = (RegexReplaceElement)manipulateChild;

            List<string> finalResults = new List<string>();
            Regex regexPattern = new Regex(regexConfig.Pattern);

            for (int i = 0; i < result.Results.Count; ++i)
            {
                finalResults.Add(Regex.Replace(result.Results[i], regexConfig.Pattern, regexConfig.Replace));
            }

            result.Results = new List<string>(finalResults);
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
