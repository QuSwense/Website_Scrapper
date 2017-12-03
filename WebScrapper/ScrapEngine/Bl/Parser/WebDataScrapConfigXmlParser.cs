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
        /// Stores the Scrap xml file data. This is the main Data to be used after parsing xml
        /// </summary>
        public List<ScrapElement> RootScrapNodes { get; set; }

        /// <summary>
        /// An intermediate reader instance
        /// </summary>
        private DXmlDocReader xmlDocReader;

        public WebDataScrapConfigXmlParser()
        {
            RootScrapNodes = new List<ScrapElement>();
        }

        /// <summary>
        /// The root function to Parse the xml file as a whole
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

            xmlDocReader = null;
        }

        /// <summary>
        /// Parse the topmost root node
        /// </summary>
        /// <param name="webDataNode"></param>
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

        /// <summary>
        /// Parse the Scrap type nodes
        /// </summary>
        /// <param name="webDataNode"></param>
        /// <param name="parentScrap"></param>
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
                else if(scrapNode.LocalName == ScrapXmlConsts.ColumnNodeName)
                    ParseColumnNode(scrapNode, parentScrap);
                else
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.UNKNOWN_NODE,
                                scrapNode.LocalName);
            }
        }

        /// <summary>
        /// Parse the column nodes
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="parentScrap"></param>
        private void ParseColumnNode(XmlNode columnNode, ScrapElement parentScrap)
        {
            var columnScrap = xmlDocReader.ReadElement<ColumnElement>(columnNode);

            columnScrap.Parent = parentScrap;
            parentScrap.Columns.Add(columnScrap);

            if (columnScrap.Parent is ScrapCsvElement)
            {
                if (columnScrap.Index < 0)
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.NODE_ATTRIBUTE_VALUE_NOT_FOUND,
                        ScrapXmlConsts.ColumnNodeName, ScrapXmlConsts.IndexAttributeName);
            }

            if (columnScrap.Parent is ScrapHtmlTableElement)
            {
                if (string.IsNullOrEmpty(columnScrap.XPath))
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.NODE_ATTRIBUTE_VALUE_NOT_FOUND,
                        ScrapXmlConsts.ColumnNodeName, ScrapXmlConsts.XPathAttributeName);
            }

            ParseColumnChildNodeList(columnNode, columnScrap);
        }

        /// <summary>
        /// Column child node list
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseColumnChildNodeList(XmlNode columnNode, ColumnElement columnScrap)
        {
            XmlNodeList columnChildNodeList = xmlDocReader.GetChildNodes(columnNode);

            if (columnChildNodeList != null)
            {
                bool bManipulateNodeEncountered = false;
                foreach (XmlNode columnChildNode in columnChildNodeList)
                {
                    if (columnChildNode.LocalName == ScrapXmlConsts.ManipulateNodeName)
                    {
                        if (bManipulateNodeEncountered)
                            throw new ScrapXmlException(ScrapXmlException.EErrorType.MANIPULATE_NODE_ONLY_ONE,
                                ScrapXmlConsts.ManipulateNodeName);

                        bManipulateNodeEncountered = true;
                        XmlNodeList manipulateChildNodeList = xmlDocReader.GetChildNodes(columnChildNode);

                        if(manipulateChildNodeList != null && manipulateChildNodeList.Count > 0)
                        {
                            foreach (XmlNode manipulateChildNode in xmlDocReader.GetChildNodes(columnChildNode))
                            {
                                ParseManipulateNode(manipulateChildNode, columnScrap);
                            }
                        }
                    }
                    else
                        throw new ScrapXmlException(ScrapXmlException.EErrorType.UNKNOWN_NODE,
                                    columnChildNode.LocalName);
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
            else if (manipulateNode.LocalName == ScrapXmlConsts.ValidateNodeName)
            {
                ValidateElement validateElement = xmlDocReader.ReadElement<ValidateElement>(manipulateNode);
                validateElement.Parent = columnScrap;
                manipulateScrap = validateElement;
            }
            else if (manipulateNode.LocalName == ScrapXmlConsts.PurgeNodeName)
            {
                PurgeElement purgeElement = xmlDocReader.ReadElement<PurgeElement>(manipulateNode);
                purgeElement.Parent = columnScrap;
                manipulateScrap = purgeElement;
            }
            else if (manipulateNode.LocalName == ScrapXmlConsts.DbchangeNodeName)
            {
                DbchangeElement dbchangeElement = xmlDocReader.ReadElement<DbchangeElement>(manipulateNode);
                dbchangeElement.Parent = columnScrap;

                ParseDbChangeElement(dbchangeElement, manipulateNode);
                manipulateScrap = dbchangeElement;
            }
            else
                throw new ScrapXmlException(ScrapXmlException.EErrorType.INVALID_MANIPULATE_CHILD_ITEM);

            manipulateScrap.Parent = columnScrap;
            columnScrap.Manipulations.Add(manipulateScrap);
        }

        private void ParseDbChangeElement(DbchangeElement dbchangeElement, XmlNode manipulateNode)
        {
            if(manipulateNode.ChildNodes != null && manipulateNode.ChildNodes.Count == 1)
            {
                DbchangeExistsElement existsElement =
                    xmlDocReader.ReadElement<DbchangeExistsElement>(manipulateNode.ChildNodes[0]);
                dbchangeElement.Exists = existsElement;
                ParseSelectElement(existsElement, manipulateNode.ChildNodes[0]);
            }
        }

        private void ParseSelectElement(DbchangeExistsElement existsElement, XmlNode xmlNode)
        {
            if (xmlNode.ChildNodes != null && xmlNode.ChildNodes.Count == 1)
            {
                DbchangeSelectElement selectElement =
                    xmlDocReader.ReadElement<DbchangeSelectElement>(xmlNode.ChildNodes[0]);
                selectElement.Parent = existsElement;
                existsElement.Select = selectElement;
            }
        }

        protected string Normalize(string htmlValue)
        {
            if (string.IsNullOrEmpty(htmlValue)) return htmlValue;
            return htmlValue.Replace(ASCIICharacters.NewLineString, ASCIICharacters.NewLine)
                .Replace(ASCIICharacters.TabString, ASCIICharacters.Tab);
        }
    }
}
