using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ScrapEngine.Model.ScrapXml;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The business logic to parse SPlit element and interprets the web scrapped data
    /// </summary>
    public class ScrapSplitConfigParser : ScrapManipulateChildConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapSplitConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        /// <summary>
        /// Process the Split element
        /// </summary>
        /// <param name="childManipulateNode"></param>
        /// <param name="webConfigObj"></param>
        /// <returns></returns>
        public override ManipulateChildElement Process(XmlNode childManipulateNode, ManipulateElement webConfigObj)
        {
            SplitElement splitElement = configParser.XmlConfigReader.ReadElement<SplitElement>(childManipulateNode);
            Assert(splitElement);
            return splitElement;
        }

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
                if (!string.IsNullOrEmpty(result.OriginalValue))
                {
                    string[] split = result.Results[i].Split(splitElement.Data.ToArray());

                    if (splitElement.Index >= 0 && splitElement.Index < split.Length)
                        finalResults.Add(split[splitElement.Index]);
                    else if(splitElement.Index < 0)
                        foreach (var item in split) finalResults.Add(item);
                }
                else
                    finalResults.Add(result.Results[i]);
            }
            result.Results = new List<string>(finalResults);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="splitElement"></param>
        private void Assert(SplitElement splitElement)
        {
            if (string.IsNullOrEmpty(splitElement.Data))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_DATA_EMPTY);
            else if (splitElement.Index < 0)
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_INDEX_INVALID,
                    new string[] { splitElement.Index.ToString() });
        }
    }
}
