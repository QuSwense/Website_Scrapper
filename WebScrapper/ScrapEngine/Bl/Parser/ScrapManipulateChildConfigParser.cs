using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// Base class for all parsers related to Manipulate child nodes
    /// </summary>
    public class ScrapManipulateChildConfigParser : AppTopicConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapManipulateChildConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }
        
        /// <summary>
        /// Virtual process
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public virtual void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild) { }
    }
}
