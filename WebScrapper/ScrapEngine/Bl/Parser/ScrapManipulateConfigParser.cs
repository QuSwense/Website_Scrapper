using HtmlAgilityPack;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// Business logic to parse and process Manipulate Tag
    /// </summary>
    public class ScrapManipulateConfigParser : AppTopicConfigParser
    {
        /// <summary>
        /// Split Manipulate tag
        /// </summary>
        private ManipulateChildFactory manipulateChildFactory;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapManipulateConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            manipulateChildFactory = new ManipulateChildFactory(configParser);
        }

        /// <summary>
        /// Process the Manipulate tags
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        public void Process(XmlNode columnNode, ColumnElement webColumnConfigObj)
        {
            XmlNodeList manipulateNodeList = columnNode.SelectNodes(ManipulateElement.TagName);

            if (manipulateNodeList != null && manipulateNodeList.Count > 0)
            {
                foreach (XmlNode manipulateNode in manipulateNodeList)
                {
                    ManipulateElement webManipulateConfigObj = new ManipulateElement();

                    foreach (XmlNode childManipulateNode in manipulateNode.ChildNodes)
                    {
                        var mChildElement =
                            manipulateChildFactory.GetParser(childManipulateNode.Name).Process(childManipulateNode, webManipulateConfigObj);
                        webManipulateConfigObj.ManipulateItems.Add(mChildElement);
                    }
                    webColumnConfigObj.Manipulations.Add(webManipulateConfigObj);
                }
            }
        }

        /// <summary>
        /// Process the Manipulate element and the scrapped website data
        /// </summary>
        /// <param name="columnConfig"></param>
        /// <param name="result"></param>
        public void Process(ColumnElement columnConfig, ManipulateHtmlData result)
        {
            if (columnConfig.Manipulations != null && columnConfig.Manipulations.Count > 0)
            {
                foreach (ManipulateElement manipulate in columnConfig.Manipulations)
                {
                    if(manipulate.ManipulateItems != null && manipulate.ManipulateItems.Count > 0)
                    {
                        foreach (var manipulateChild in manipulate.ManipulateItems)
                        {
                            manipulateChildFactory.GetParser(manipulateChild).Process(result, manipulateChild);
                        }
                    }
                }
            }

            result.XPath = columnConfig.XPath;
            result.Value = HtmlEntity.DeEntitize(result.Value);
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseManipulateChildElement(XmlNode manipulateNode, 
            ManipulateElement webManipulateConfigObj)
        {
            foreach (XmlNode childManipulateNode in manipulateNode.ChildNodes)
            {
                var configManipulate =
                    manipulateChildFactory.Create(childManipulateNode.Name);

                var configManipulateParser = manipulateChildFactory.GetParser(childManipulateNode.Name);
                configManipulateParser.Process(childManipulateNode, webManipulateConfigObj);

                webManipulateConfigObj.ManipulateItems.Add(configManipulate);
            }
        }
    }
}
