using ScrapEngine.Common;
using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

        protected void ParseScrapNode(XmlNode webDataNode, ScrapElement parentScrap = null)
        {
            foreach (XmlNode scrapNode in xmlDocReader.GetChildNodes(webDataNode))
            {
                ScrapElement currentScrap = null;
                if (scrapNode.LocalName == ScrapXmlConsts.ScrapHtmlTableNodeName)
                    currentScrap = xmlDocReader.ReadElement<ScrapHtmlTableElement>(scrapNode);
                else if (scrapNode.LocalName == ScrapXmlConsts.ScrapCsvNodeName)
                    currentScrap = xmlDocReader.ReadElement<ScrapCsvElement>(scrapNode);
                else
                    ParseScrapChildNode(scrapNode, parentScrap);

                currentScrap.Parent = parentScrap;
                parentScrap.Scraps.Add(currentScrap);
                ParseScrapNode(scrapNode, currentScrap);
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
                if (manipulateNodeList.Count != 1)
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.CHILD_NODE_PER_NODE_COUNT, "1",
                        ScrapXmlConsts.ManipulateNodeName, ScrapXmlConsts.ColumnNodeName);

                foreach (XmlNode manipulateNode in manipulateNodeList)
                {
                    if (manipulateNode.LocalName == ScrapXmlConsts.ManipulateNodeName)
                        ParseManipulateNode(manipulateNode, columnScrap);
                }
            }
        }

        private void ParseManipulateNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            ManipulateChildElement manipulateScrap = null;

            if (manipulateNode.LocalName == ScrapXmlConsts.SplitNodeName)
                manipulateScrap = xmlDocReader.ReadElement<SplitElement>(manipulateNode);
            else if (manipulateNode.LocalName == ScrapXmlConsts.TrimNodeName)
                manipulateScrap = xmlDocReader.ReadElement<TrimElement>(manipulateNode);
            else if (manipulateNode.LocalName == ScrapXmlConsts.RegexNodeName)
                manipulateScrap = xmlDocReader.ReadElement<RegexElement>(manipulateNode);
            else if (manipulateNode.LocalName == ScrapXmlConsts.ReplaceNodeName)
                manipulateScrap = xmlDocReader.ReadElement<ReplaceElement>(manipulateNode);
            else if (manipulateNode.LocalName == ScrapXmlConsts.RegexReplaceNodeName)
                manipulateScrap = xmlDocReader.ReadElement<RegexReplaceElement>(manipulateNode);
            else
                throw new ScrapXmlException(ScrapXmlException.EErrorType.INVALID_MANIPULATE_CHILD_ITEM);

            manipulateScrap.Parent = columnScrap;
            columnScrap.Manipulations.Add(manipulateScrap);
        }
    }
}
