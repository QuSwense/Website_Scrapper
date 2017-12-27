using ScrapEngine.Model;
using System.Collections.Generic;
using System.Linq;
using ScrapEngine.Model.Scrap;
using WebCommon.Error;
using System;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The business logic to parse SPlit element and interprets the web scrapped data
    /// </summary>
    public class ScrapSplitConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapSplitConfigParser() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapSplitConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            SplitElement splitElement = (SplitElement)manipulateChild;

            List<string> finalResults = new List<string>();
            for (int i = 0; i < result.Results.Count; ++i)
            {
                if (!string.IsNullOrEmpty(result.Results[i]))
                {
                    string[] split = null;

                    if(splitElement.SplitAsString)
                        split = result.Results[i].Split(new string[] { splitElement.Data }, StringSplitOptions.None);
                    else
                        split = result.Results[i].Split(splitElement.Data.ToArray());

                    if (splitElement.Index >= 0 && splitElement.Index < split.Length)
                        finalResults.Add(split[splitElement.Index]);
                    else if (splitElement.Index == -1) // Add all
                        foreach (var item in split) finalResults.Add(item);
                    else if (splitElement.Index == -2) // Add last
                        finalResults.Add(split.LastOrDefault());
                    else
                        throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_INDEX_INVALID);
                }
                else
                    finalResults.Add(result.Results[i]);
            }
            result.Results = new List<string>(finalResults);
        }

        /// <summary>
        /// Assert the split element data
        /// </summary>
        /// <param name="splitElement"></param>
        private void Assert(SplitElement splitElement)
        {
            if (string.IsNullOrEmpty(splitElement.Data))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_DATA_EMPTY);
            else if (!(splitElement.Index == -1 || splitElement.Index == -2 || splitElement.Index >= 0))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_INDEX_INVALID,
                    new string[] { splitElement.Index.ToString() });
        }
    }
}
