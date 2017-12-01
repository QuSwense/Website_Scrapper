using ScrapEngine.Common;
using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebCommon.Const;
using WebCommon.Error;
using WebReader.Xml;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// Parses and stores the Xml Scrap config xml file data in memory
    /// </summary>
    public class WebDataScrapConfigXmlParser
    {
        /// <summary>
        /// Stores the Scrap xml file data
        /// </summary>
        public List<ScrapElement> RootScrapNodes { get; set; }

        private DXmlDocReader xmlDocReader;

        public WebDataScrapConfigXmlParser()
        {
            RootScrapNodes = new List<ScrapElement>();
        }

        /// <summary>
        /// Parse the xml file
        /// </summary>
        public void Parse(string configFile)
        {
            xmlDocReader = new DXmlDocReader();
            xmlDocReader.Initialize(configFile);

            foreach (XmlNode webDataNode in xmlDocReader.GetChildNodes())
            {
                if (webDataNode is XmlDeclaration) continue;

                if (webDataNode.LocalName != ScrapXmlConsts.WebDataNodeName)
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.NODE_NOT_FOUND,
                        ScrapXmlConsts.WebDataNodeName);
                ParseRootScrapNode(webDataNode);
            }
        }

        protected void ParseRootScrapNode(XmlNode webDataNode)
        {
            foreach (XmlNode scrapNode in xmlDocReader.GetChildNodes(webDataNode))
            {
                ScrapElement currentScrap = null;
                if (scrapNode.LocalName == ScrapXmlConsts.ScrapHtmlTableNodeName)
                    currentScrap = xmlDocReader.ReadElement<ScrapHtmlTableElement>(scrapNode);
                else if (scrapNode.LocalName == ScrapXmlConsts.ScrapCsvNodeName)
                    currentScrap = xmlDocReader.ReadElement<ScrapCsvElement>(scrapNode);
                else
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.UNKNOWN_NODE,
                            scrapNode.LocalName);

                // id and name attribute is mnadatory in the root scrap node
                if (currentScrap == null)
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.PARSE_NODE_ERROR);

                if (string.IsNullOrEmpty(currentScrap.IdString))
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.MANDATORY_ATTRIBUTE_NOT_FOUND,
                        ScrapXmlConsts.ScrapHtmlTableNodeName, ScrapXmlConsts.IdAttributeName);

                if (string.IsNullOrEmpty(currentScrap.Name))
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.MANDATORY_ATTRIBUTE_NOT_FOUND,
                        ScrapXmlConsts.ScrapHtmlTableNodeName, ScrapXmlConsts.NameAttributeName);

                RootScrapNodes.Add(currentScrap);
                ParseScrapNode(scrapNode, currentScrap);
            }
        }

        protected void ParseScrapNode(XmlNode webDataNode, ScrapElement parentScrap)
        {
            foreach (XmlNode scrapNode in xmlDocReader.GetChildNodes(webDataNode))
            {
                ScrapElement currentScrap = null;
                if (scrapNode.LocalName == ScrapXmlConsts.ScrapHtmlTableNodeName)
                {
                    currentScrap = xmlDocReader.ReadElement<ScrapHtmlTableElement>(scrapNode);
                    currentScrap.Parent = parentScrap;
                    parentScrap.Scraps.Add(currentScrap);
                    ParseScrapNode(scrapNode, currentScrap);
                }
                else if (scrapNode.LocalName == ScrapXmlConsts.ScrapCsvNodeName)
                {
                    ScrapCsvElement currentCsvScrap = xmlDocReader.ReadElement<ScrapCsvElement>(scrapNode);
                    currentCsvScrap.Delimiter = Normalize(currentCsvScrap.Delimiter);
                    currentScrap = currentCsvScrap;
                    currentScrap.Parent = parentScrap;
                    parentScrap.Scraps.Add(currentScrap);
                    ParseScrapNode(scrapNode, currentScrap);
                }
                else
                    ParseScrapChildNode(scrapNode, parentScrap);
            }
        }

        private void ParseScrapChildNode(XmlNode columnNode, ScrapElement parentScrap)
        {
            if (columnNode.LocalName == ScrapXmlConsts.ColumnNodeName)
            {
                var columnScrap = xmlDocReader.ReadElement<ColumnElement>(columnNode);

                columnScrap.Parent = parentScrap;
                ParseColumnNode(columnNode, columnScrap);
            }
            else
                throw new ScrapXmlException(ScrapXmlException.EErrorType.UNKNOWN_NODE,
                            columnNode.LocalName);
        }

        private void ParseColumnNode(XmlNode columnNode, ColumnElement columnScrap)
        {
            if(columnScrap.Parent is ScrapCsvElement)
            {
                if(columnScrap.Index < 0)
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.NODE_ATTRIBUTE_VALUE_NOT_FOUND,
                        ScrapXmlConsts.ColumnNodeName, ScrapXmlConsts.IndexAttributeName);
            }

            if (columnScrap.Parent is ScrapHtmlTableElement)
            {
                if (string.IsNullOrEmpty(columnScrap.XPath))
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.NODE_ATTRIBUTE_VALUE_NOT_FOUND,
                        ScrapXmlConsts.ColumnNodeName, ScrapXmlConsts.XPathAttributeName);
            }

            XmlNodeList manipulateNodeList = xmlDocReader.GetChildNodes(columnNode);

            if (manipulateNodeList != null)
            {
                foreach (XmlNode manipulateNode in manipulateNodeList)
                {
                    if (manipulateNode.LocalName == ScrapXmlConsts.ManipulateNodeName)
                    {
                        XmlNodeList manipulateChildNodeList = xmlDocReader.GetChildNodes(manipulateNode);

                        if(manipulateChildNodeList != null && manipulateChildNodeList.Count > 0)
                        {
                            foreach (XmlNode manipulateChildNode in xmlDocReader.GetChildNodes(manipulateNode))
                            {
                                ParseManipulateNode(manipulateChildNode, columnScrap);
                            }
                        }
                    }
                }
            }
        }

        private void ParseManipulateNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            ManipulateChildElement manipulateScrap = null;

            if (manipulateNode.LocalName == ScrapXmlConsts.SplitNodeName)
            {
                SplitElement splitElement = xmlDocReader.ReadElement<SplitElement>(manipulateNode);
                splitElement.Data = Normalize(splitElement.Data);
                manipulateScrap = splitElement;
            }
            else if (manipulateNode.LocalName == ScrapXmlConsts.TrimNodeName)
            {
                TrimElement trimElement = xmlDocReader.ReadElement<TrimElement>(manipulateNode);
                trimElement.Data = Normalize(trimElement.Data);
                manipulateScrap = trimElement;
            }
            else if (manipulateNode.LocalName == ScrapXmlConsts.RegexNodeName)
            {
                RegexElement regexElement = xmlDocReader.ReadElement<RegexElement>(manipulateNode);
                regexElement.Pattern = Normalize(regexElement.Pattern);
                manipulateScrap = regexElement;
            }
            else if (manipulateNode.LocalName == ScrapXmlConsts.ReplaceNodeName)
            {
                ReplaceElement replaceElement = xmlDocReader.ReadElement<ReplaceElement>(manipulateNode);
                replaceElement.InString = Normalize(replaceElement.InString);
                replaceElement.OutString = Normalize(replaceElement.OutString);
                manipulateScrap = replaceElement;
            }
            else if (manipulateNode.LocalName == ScrapXmlConsts.RegexReplaceNodeName)
            {
                RegexReplaceElement regexReplaceElement = xmlDocReader.ReadElement<RegexReplaceElement>(manipulateNode);
                regexReplaceElement.Pattern = Normalize(regexReplaceElement.Pattern);
                regexReplaceElement.Replace = Normalize(regexReplaceElement.Replace);
                manipulateScrap = regexReplaceElement;
            }
            else
                throw new ScrapXmlException(ScrapXmlException.EErrorType.INVALID_MANIPULATE_CHILD_ITEM);

            manipulateScrap.Parent = columnScrap;
            columnScrap.Manipulations.Add(manipulateScrap);
        }

        protected string Normalize(string htmlValue)
        {
            if (string.IsNullOrEmpty(htmlValue)) return htmlValue;
            return htmlValue.Replace(ASCIICharacters.NewLineString, ASCIICharacters.NewLine)
                .Replace(ASCIICharacters.TabString, ASCIICharacters.Tab);
        }
    }
}
