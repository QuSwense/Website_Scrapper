using log4net;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System;
using System.Xml.XPath;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The base parser class for Scrap node
    /// </summary>
    public class ScrapConfigParser : AppTopicConfigParser
    {
        #region Properties

        /// <summary>
        /// Logger
        /// </summary>
        public static ILog logger = LogManager.GetLogger(typeof(ScrapHtmlTableConfigParser));

        #endregion Properties

        #region Helper Properties

        /// <summary>
        /// Stores the current state which is getting processed. Save the State before
        /// sending to process child node
        /// </summary>
        private ScrapStateModel currentState
        {
            get
            {
                return configParserTemplate.StateModel.PeekScrap();
            }
        }

        #endregion Helper Properties

        /// <summary>
        /// Scrap column config parser
        /// </summary>
        protected DbRowConfigParser dbConfigConfigParser;

        public ScrapConfigParser() { }

        /// <summary>
        /// Start Processing from the Scrap Html node
        /// </summary>
        public virtual void Process() { }

        ///// <summary>
        ///// Check the maximum level of Scrap nodes allowed is 4
        ///// </summary>
        ///// <param name="webScrapConfigObj">The last child Scrap node</param>
        //protected void AssertLevelConstraint()
        //{
        //    ScrapElement tmpObj = configParser.StateModel.CurrentColumnScrapIteratorArgs.Parent.ScrapConfigObj;
        //    int level = 0;
        //    for (; tmpObj != null && level <= configParser.AppConfig.ScrapMaxLevel();
        //        level++, tmpObj = tmpObj.Parent) ;

        //    if (level > configParser.AppConfig.ScrapMaxLevel() || level <= 0)
        //        throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_LEVEL_INVALID,
        //            level.ToString());
        //}

        ///// <summary>
        ///// The Scrap element tag (and its child Scrap tags) should contain one and only one name 
        ///// attribute
        ///// </summary>
        ///// <param name="webScrapConfigObj"></param>
        //protected void AssertScrapNameAttribute()
        //{
        //    bool isTableNameFound = false;
        //    string NameValue = null;
        //    ScrapElement tmpObj = configParser.StateModel.CurrentColumnScrapIteratorArgs.Parent.ScrapConfigObj;

        //    while (tmpObj != null)
        //    {
        //        if (!string.IsNullOrEmpty(tmpObj.Name))
        //        {
        //            if (isTableNameFound)
        //                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_MULTIPLE);
        //            isTableNameFound = true;
        //            NameValue = tmpObj.Name;
        //        }

        //        tmpObj = tmpObj.Parent;
        //    }

        //    if (!isTableNameFound || string.IsNullOrEmpty(NameValue))
        //        throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_EMPTY);
        //}

        /// <summary>
        /// Parse url value
        /// </summary>
        /// <param name="urlValue"></param>
        /// <param name="scrapNode"></param>
        /// <returns></returns>
        protected void ParseUrlValue()
        {
            ScrapStateModel currentState = this.currentState;
            ScrapElement scrapObj = ((ScrapElement)currentState.Config);
            ScrapElement scrapParentObj = ((ScrapElement)currentState.Parent.Config);

            if (scrapParentObj == null ||
                string.IsNullOrEmpty(scrapParentObj.UrlOriginal)) return;
            if (!string.IsNullOrEmpty(scrapParentObj.UrlOriginal)
                && scrapParentObj.UrlOriginal.Contains("{parentValue}"))
            {
                currentState.AbsoluteUrl = scrapParentObj.UrlOriginal.Replace("{parentValue}", "");

                if (string.IsNullOrEmpty(scrapObj.UrlOriginal))
                {
                    currentState.AbsoluteUrl = new Uri(new Uri(scrapParentObj.UrlOriginal),
                    ((ScrapStateModel)currentState.Parent).WebHtmlNode.Value).AbsoluteUri;
                }
                else
                {
                    XPathNavigator htmlNode =
                        ((ScrapStateModel)currentState.Parent).WebHtmlNode.SelectSingleNode(
                            currentState.AbsoluteUrl);
                    currentState.AbsoluteUrl = new Uri(new Uri(scrapParentObj.Url),
                    htmlNode.Value).AbsoluteUri;
                }
            }
            
            currentState.AbsoluteUrl = urlValue;
        }

        /// <summary>
        /// A common method to call COlumn parser with arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        protected void ProcessColumnParser()
        {
            // Check the constraints on the Scrap nodes
            // 1. Only maximum 4 levels is allowed
            // 2. Only one "name" tag should be present from the top level to bottom Scrap
            //    If multiple "name" tag is present throw error
            //AssertLevelConstraint();
            //AssertScrapNameAttribute();

            // Read the Column nodes which are the individual reader config nodes
            //scrapColumnConfigParser.Process();
        }
    }
}
