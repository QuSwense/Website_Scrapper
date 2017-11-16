using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Bl
{
    public class ScrapColumnConfigParser : IInnerBaseParser
    {
        protected IInnerBaseParser parentParser;
        protected WebScrapConfigParser configParser;
        protected WebScrapParserColumnStateModel startState;

        public ScrapColumnConfigParser(WebScrapConfigParser configParser,
            WebScrapParserColumnStateModel startState, IInnerBaseParser parentParser)
        {
            this.configParser = configParser;
            this.startState = startState;
            this.parentParser = parentParser;
        }

        public void Run()
        {
            if (startState.ScrapState.ConfigScrap.Columns.Count <= 0)
            {
                XmlNodeList columnNodeList = startState.ScrapState.CurrentXmlNode.SelectNodes("Column");

                if (columnNodeList != null && columnNodeList.Count > 0)
                {
                    foreach (XmlNode columnNode in columnNodeList)
                    {
                        var stateModel = new WebScrapParserColumnStateModel()
                        {
                            NodeIndex = startState.NodeIndex,
                            ScrapState = startState.ScrapState
                        };

                        stateModel.ScrapState.CurrentXmlNode = columnNode;
                        startState.ScrapState.ConfigScrap.Columns.Add(
                            ParseColumnElement(stateModel));
                    }
                }

                // Load the table with partial columns in memory
                configParser.WebDbContext.AddMetadata(startState.ScrapState.ConfigScrap);
            }

            ColumnScrapIterator(nodeIndex, webScrapConfigObj, scrapNode, webNodeNavigator);
        }

        /// <summary>
        /// Parse the Column tag in config file
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private WebDataConfigColumn ParseColumnElement(WebScrapParserColumnStateModel state)
        {
            WebDataConfigColumn webColumnConfigObj = ParseColumnElementAttributes(state);

            // Check if manipulate tag is present
            ParseManipulateElement(columnNode, webColumnConfigObj);

            return webColumnConfigObj;
        }

        /// <summary>
        /// Parse the Config Xml Column attribute
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private WebDataConfigColumn ParseColumnElementAttributes(XmlNode columnNode, WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            WebDataConfigColumn webColumnConfigObj = XmlConfigReader.ReadElement<WebDataConfigColumn>(columnNode);
            webColumnConfigObj.Parent = webScrapConfigObj;
            webColumnConfigObj.Name = webColumnConfigObj.Name;

            // If the parent Scrap node is 'TABLE' type then only xpath is valid and mandatory
            if (webScrapConfigObj.Type == EWebDataConfigType.TABLE &&
                string.IsNullOrEmpty(webColumnConfigObj.XPath))
                throw new Exception("XPath must be specified for TABLE type scrapping");
            else if (webScrapConfigObj.Type == EWebDataConfigType.CSV)
            {
                if (string.IsNullOrEmpty(columnNode.Attributes["index"].Value))
                    throw new Exception("Index must be specified for CSV type scrapping");
                webColumnConfigObj.Index = Convert.ToInt32(columnNode.Attributes["index"].Value);
            }

            return webColumnConfigObj;
        }
    }
}
