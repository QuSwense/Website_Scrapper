using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System.Collections.Generic;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapValidateConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapValidateConfigParser(WebScrapConfigParser configParser) 
            : base(configParser) { }

        /// <summary>
        /// Process multiple results
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public void Process(ManipulateHtmlData manipulateHtml, List<ValidateElement> validations)
        {
            foreach (var validateConfig in validations)
            {
                foreach (string value in manipulateHtml.Results)
                {
                    int count = configParser.WebDbContext.ValidateExists(validateConfig.Table,
                        validateConfig.Column, value);

                    if (count <= 0)
                        throw new ScrapParserException(ScrapParserException.EErrorType.VALIDATION_FAILED,
                            validateConfig.Table, validateConfig.Column, value);
                }
            }
        }
    }
}
