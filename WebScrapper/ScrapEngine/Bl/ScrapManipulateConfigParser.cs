using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// Business logic to parse and process Manipulate Tag
    /// </summary>
    public class ScrapManipulateConfigParser : IInnerBaseParser
    {
        protected WebScrapConfigParser configParser;

        public ScrapManipulateConfigParser(WebScrapConfigParser configParser)
        {
            this.configParser = configParser;
        }

        public void Process(XmlNode columnNode, WebDataConfigColumn webColumnConfigObj)
        {
            XmlNodeList manipulateNodeList = columnNode.SelectNodes("Manipulate");

            if (manipulateNodeList != null && manipulateNodeList.Count > 0)
            {
                foreach (XmlNode manipulateNode in manipulateNodeList)
                {
                    WebDataConfigManipulate webManipulateConfigObj = new WebDataConfigManipulate();
                    ParseManipulateChildElement(manipulateNode, webManipulateConfigObj);
                    webColumnConfigObj.Manipulations.Add(webManipulateConfigObj);
                }
            }
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseManipulateChildElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            ParseSplitElement(manipulateNode, webManipulateConfigObj);
            ParseTrimElement(manipulateNode, webManipulateConfigObj);
            ParseReplaceElement(manipulateNode, webManipulateConfigObj);
            ParseRegexElement(manipulateNode, webManipulateConfigObj);
        }

        private void ParseRegexElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            XmlNodeList splitNodeList = manipulateNode.SelectNodes("Regex");

            if (splitNodeList != null && splitNodeList.Count > 0)
            {
                foreach (XmlNode splitNode in splitNodeList)
                {
                    WebDataConfigRegex configRegexObj = configParser.XmlConfigReader.ReadElement<WebDataConfigRegex>(splitNode);
                    configRegexObj.Pattern = HttpUtility.HtmlDecode(configRegexObj.Pattern);
                    webManipulateConfigObj.Regexes.Add(configRegexObj);
                }
            }
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseSplitElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            XmlNodeList splitNodeList = manipulateNode.SelectNodes("Split");

            if (splitNodeList != null && splitNodeList.Count > 0)
            {
                foreach (XmlNode splitNode in splitNodeList)
                {
                    WebDataConfigSplit configSpliObj = configParser.XmlConfigReader.ReadElement<WebDataConfigSplit>(splitNode);
                    webManipulateConfigObj.Splits.Add(configSpliObj);
                }
            }
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseTrimElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            XmlNodeList trimNodeList = manipulateNode.SelectNodes("Trim");

            if (trimNodeList != null && trimNodeList.Count > 0)
            {
                foreach (XmlNode trimNode in trimNodeList)
                {
                    WebDataConfigTrim configTrimObj = configParser.XmlConfigReader.ReadElement<WebDataConfigTrim>(trimNode);
                    webManipulateConfigObj.Trims.Add(configTrimObj);
                    configTrimObj.Data = Normalize(configTrimObj.Data);
                }
            }
        }

        /// <summary>
        /// Parse the manipulate element tag
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webColumnConfigObj"></param>
        private void ParseReplaceElement(XmlNode manipulateNode, WebDataConfigManipulate webManipulateConfigObj)
        {
            // Check if split tag is present
            XmlNodeList replaceNodeList = manipulateNode.SelectNodes("Replace");

            if (replaceNodeList != null && replaceNodeList.Count > 0)
            {
                foreach (XmlNode replaceNode in replaceNodeList)
                {
                    WebDataConfigReplace configTrimObj = configParser.XmlConfigReader.ReadElement<WebDataConfigReplace>(replaceNode);
                    webManipulateConfigObj.Replaces.Add(configTrimObj);
                    configTrimObj.InString = Normalize(configTrimObj.InString);
                    configTrimObj.OutString = Normalize(configTrimObj.OutString);
                }
            }
        }

        private string Normalize(string data)
        {
            if (string.IsNullOrEmpty(data)) return data;
            return data.Replace("\\n", "\n").Replace("\\t", "\t");
        }
    }
}
