using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System.Collections.Generic;
using System.Xml;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapReplaceConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapReplaceConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }
        
        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            if (string.IsNullOrEmpty(result.OriginalValue)) return;

            ReplaceElement replaceConfig = (ReplaceElement)manipulateChild;

            List<string> finalResults = new List<string>();
            for (int i = 0; i < result.Results.Count; ++i)
                finalResults.Add(result.Results[i].Replace(replaceConfig.InString, replaceConfig.OutString));

            result.Results = new List<string>(finalResults);
        }

        private void Assert(ReplaceElement replaceElement)
        {
            if (string.IsNullOrEmpty(replaceElement.InString))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_REPLACE_INSTRING_EMPTY);
        }
    }
}
