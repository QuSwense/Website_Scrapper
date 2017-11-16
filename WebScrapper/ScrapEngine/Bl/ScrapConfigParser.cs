using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCommon.Error;

namespace ScrapEngine.Bl
{
    public class ScrapConfigParser : IInnerBaseParser
    {
        protected WebScrapConfigParser configParser;

        public ScrapConfigParser(WebScrapConfigParser configParser)
        {
            this.configParser = configParser;
        }

        /// <summary>
        /// Check the maximum level of Scrap nodes allowed is 4
        /// </summary>
        /// <param name="webScrapConfigObj">The last child Scrap node</param>
        protected void CheckMaxLevelConstraint(WebDataConfigScrap webScrapConfigObj)
        {
            WebDataConfigScrap tmpObj = webScrapConfigObj;
            int level = 0;
            while (tmpObj != null)
            {
                level++;
                tmpObj = tmpObj.Parent;
            }

            if (level > 4 || level <= 0)
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_LEVEL_INVALID,
                    level.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        protected void CheckScrapNameAttribute(WebDataConfigScrap webScrapConfigObj)
        {
            bool isTableNameFound = false;
            string NameValue = null;
            WebDataConfigScrap tmpObj = webScrapConfigObj;

            while (tmpObj != null)
            {
                if (!string.IsNullOrEmpty(tmpObj.Name))
                {
                    if (isTableNameFound)
                        throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_MULTIPLE);
                    isTableNameFound = true;
                    NameValue = tmpObj.Name;
                }

                tmpObj = tmpObj.Parent;
            }

            if (!isTableNameFound)
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_EMPTY);
        }
    }
}
