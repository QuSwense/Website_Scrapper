using ScrapEngine.Common;
using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using WebCommon.Const;
using WebCommon.Error;
using WebReader.Model;
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
        public WebDataElement WebDataRoot { get; set; }

        /// <summary>
        /// An intermediate reader instance
        /// </summary>
        private DXmlDocReader xmlDocReader;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebDataScrapConfigXmlParser()
        {
            WebDataRoot = new WebDataElement();
        }

        /// <summary>
        /// The root function to Parse the xml file as a whole
        /// </summary>
        public void Parse(string configFile)
        {
            xmlDocReader = new DXmlDocReader();
            xmlDocReader.Initialize(configFile);

            mapProperties = new Dictionary<string, XmlPropertyMap>();
            mapElementProperties = new Dictionary<string, XmlElementPropertyMap>();

            Type typeNode = typeof(WebDataElement);
            List<DXmlElementAttribute> elementAttributes = typeNode.GetCustomAttributes<DXmlElementAttribute>().ToList();

            if (elementAttributes == null || elementAttributes.Count <= 0)
                throw new Exception();

            foreach (XmlNode webDataNode in xmlDocReader.GetChildNodes())
            {
                if (webDataNode is XmlDeclaration) continue;

                DXmlElementAttribute elementAttribute = elementAttributes.Where(p => p.Name == webDataNode.LocalName).First();
                if (elementAttribute == null)
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.NODE_NOT_FOUND,
                        ScrapXmlConsts.WebDataNodeName);
                ParseRootScrapNode(webDataNode, WebDataRoot);
            }

            xmlDocReader = null;
        }

        /// <summary>
        /// Parse the topmost root node
        /// </summary>
        /// <param name="webDataNode"></param>
        protected void ParseRootScrapNode(XmlNode webDataNode, WebDataElement webDataRoot)
        {
            webDataRoot = xmlDocReader.ReadElement<WebDataElement>(webDataNode);
            CreatePropertyMap(webDataRoot);

            foreach (XmlNode scrapNode in xmlDocReader.GetChildNodes(webDataNode))
            {
                ScrapElement currentScrap = null;

                if (mapProperties.ContainsKey(scrapNode.LocalName))
                {

                }
                else if(mapElementProperties.ContainsKey(scrapNode.LocalName))
                {
                    currentScrap = (ScrapElement)xmlDocReader.ReadElement(scrapNode, 
                        mapElementProperties[scrapNode.LocalName].ElementAttriibute.DerivedType);
                    SetElementValue(scrapNode.LocalName, currentScrap);

                    ParseScrapChildNode(scrapNode, currentScrap);
                }

                //    if (scrapNode.LocalName == ScrapXmlConsts.ScrapHtmlTableNodeName)
                //    currentScrap = xmlDocReader.ReadElement<ScrapHtmlTableElement>(scrapNode);
                //else if (scrapNode.LocalName == ScrapXmlConsts.ScrapCsvNodeName)
                //    currentScrap = xmlDocReader.ReadElement<ScrapCsvElement>(scrapNode);
                //else
                //    throw new ScrapXmlException(ScrapXmlException.EErrorType.UNKNOWN_NODE,
                //            scrapNode.LocalName);

                //// id and name attribute is mnadatory in the root scrap node
                //if (currentScrap == null)
                //    throw new ScrapXmlException(ScrapXmlException.EErrorType.PARSE_NODE_ERROR);

                //if (string.IsNullOrEmpty(currentScrap.IdString))
                //    throw new ScrapXmlException(ScrapXmlException.EErrorType.MANDATORY_ATTRIBUTE_NOT_FOUND,
                //        ScrapXmlConsts.ScrapHtmlTableNodeName, ScrapXmlConsts.IdAttributeName);

                //if (string.IsNullOrEmpty(currentScrap.Name))
                //    throw new ScrapXmlException(ScrapXmlException.EErrorType.MANDATORY_ATTRIBUTE_NOT_FOUND,
                //        ScrapXmlConsts.ScrapHtmlTableNodeName, ScrapXmlConsts.NameAttributeName);

                //if (currentScrap is ScrapHtmlTableElement &&
                //string.IsNullOrEmpty(currentScrap.UrlOriginal))
                //    throw new ScrapXmlException(ScrapXmlException.EErrorType.MANDATORY_ATTRIBUTE_NOT_FOUND,
                //            ScrapXmlConsts.ScrapHtmlTableNodeName, ScrapXmlConsts.UrlAttributeName);

                //webDataRoot.Scraps.Add(currentScrap);
                //ParseScrapChildNode(scrapNode, currentScrap);
            }
        }

        private void SetElementValue(string localName, ScrapElement currentScrap)
        {
            XmlElementPropertyMap elementPropMap = mapElementProperties[localName];

            if(elementPropMap.PropInfo.PropertyType == typeof(List<>))
            {
                object listObj = elementPropMap.PropInfo.GetValue(elementPropMap.webDataRoot, null);
                elementPropMap.PropInfo.PropertyType.GetMethod("Add").Invoke(listObj, new[] { currentScrap });
            }
        }

        Dictionary<string, XmlPropertyMap> mapProperties;
        Dictionary<string, XmlElementPropertyMap> mapElementProperties;

        private void CreatePropertyMap(WebDataElement webDataRoot)
        {
            Type typeNode = webDataRoot.GetType();
            PropertyInfo[] PropInfos = typeNode.GetProperties(BindingFlags.Instance);
            List<PropertyInfo> reqPropInfos = new List<PropertyInfo>();

            foreach (var item in PropInfos)
            {
                List<DXmlAttributeAttribute> attrAttributes = item.GetCustomAttributes<DXmlAttributeAttribute>().ToList();
                if (attrAttributes != null)
                {
                    foreach (var attrAttribute in attrAttributes)
                    {
                        mapProperties.Add(attrAttribute.Name, new XmlPropertyMap()
                        {
                            AttrAttriibute = attrAttribute,
                            PropInfo = item,
                            webDataRoot = webDataRoot
                        });
                    }
                }

                List<DXmlElementAttribute> elementAttributes = item.GetCustomAttributes<DXmlElementAttribute>().ToList();
                if (elementAttributes != null)
                {
                    foreach (var elementAttribute in elementAttributes)
                    {
                        mapElementProperties.Add(elementAttribute.Name, new XmlElementPropertyMap()
                        {
                            ElementAttriibute = elementAttribute,
                            PropInfo = item,
                            webDataRoot = webDataRoot
                        });
                    }
                }
            }
        }

        internal class XmlPropertyMap
        {
            public PropertyInfo PropInfo { get; set; }
            public DXmlAttributeAttribute AttrAttriibute { get; set; }

            public WebDataElement webDataRoot;
        }

        internal class XmlElementPropertyMap
        {
            public PropertyInfo PropInfo { get; set; }
            public DXmlElementAttribute ElementAttriibute { get; set; }

            public WebDataElement webDataRoot;
        }

        /// <summary>
        /// Parse the Scrap type nodes
        /// </summary>
        /// <param name="webDataNode"></param>
        /// <param name="parentScrap"></param>
        protected void ParseScrapChildNode(XmlNode webDataNode, ScrapElement parentScrap)
        {
            foreach (XmlNode scrapNode in xmlDocReader.GetChildNodes(webDataNode))
            {
                ScrapElement currentScrap = null;
                if (scrapNode.LocalName == ScrapXmlConsts.ScrapHtmlTableNodeName)
                {
                    currentScrap = ParseScrapNode<ScrapHtmlTableElement>(scrapNode, parentScrap);
                }
                else if (scrapNode.LocalName == ScrapXmlConsts.ScrapCsvNodeName)
                {
                    currentScrap = ParseScrapNode<ScrapCsvElement>(scrapNode, parentScrap);
                    ((ScrapCsvElement)currentScrap).Delimiter = 
                        Normalize(((ScrapCsvElement)currentScrap).Delimiter);
                }
                else if(scrapNode.LocalName == ScrapXmlConsts.ColumnNodeName)
                    ParseColumnNode(scrapNode, parentScrap);
                else
                    throw new ScrapXmlException(ScrapXmlException.EErrorType.UNKNOWN_NODE,
                                scrapNode.LocalName);
            }
        }

        /// <summary>
        /// Parser the recursive Scrap type nodes
        /// </summary>
        /// <param name="webDataNode"></param>
        /// <param name="parentScrap"></param>
        private ScrapElement ParseScrapNode<T>(XmlNode scrapNode, ScrapElement parentScrap)
            where T : ScrapElement, new()
        {
            ScrapElement currentScrap = xmlDocReader.ReadElement<T>(scrapNode);
            currentScrap.Parent = parentScrap;
            parentScrap.Scraps.Add(currentScrap);
            ParseScrapChildNode(scrapNode, currentScrap);
            
            return currentScrap;
        }

        /// <summary>
        /// Parse the column nodes
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="parentScrap"></param>
        private void ParseColumnNode(XmlNode columnNode, ScrapElement parentScrap)
        {
            var columnScrap = xmlDocReader.ReadElement<ColumnElement>(columnNode);

            if(!string.IsNullOrEmpty(columnScrap.SkipIfValueString))
            {
                columnScrap.SkipIfValues = columnScrap.SkipIfValueString.Split(new char[] { ';' }).ToList();
            }

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
                            foreach (XmlNode manipulateChildNode in
                                xmlDocReader.GetChildNodes(columnChildNode))
                                ParseManipulateNode(manipulateChildNode, columnScrap);
                    }
                    else
                        throw new ScrapXmlException(ScrapXmlException.EErrorType.UNKNOWN_NODE,
                                    columnChildNode.LocalName);
                }
            }
        }

        /// <summary>
        /// Parse and interpret the Manipulate child nodes
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            if (manipulateNode.LocalName == ScrapXmlConsts.SplitNodeName)
                ParseManipulateSplitNode(manipulateNode, columnScrap);
            else if (manipulateNode.LocalName == ScrapXmlConsts.TrimNodeName)
                ParseManipulateTrimNode(manipulateNode, columnScrap);
            else if (manipulateNode.LocalName == ScrapXmlConsts.RegexNodeName)
                ParseManipulateRegexNode(manipulateNode, columnScrap);
            else if (manipulateNode.LocalName == ScrapXmlConsts.ReplaceNodeName)
                ParseManipulateReplaceNode(manipulateNode, columnScrap);
            else if (manipulateNode.LocalName == ScrapXmlConsts.RegexReplaceNodeName)
                ParseManipulateRegexReplaceNode(manipulateNode, columnScrap);
            else if (manipulateNode.LocalName == ScrapXmlConsts.ValidateNodeName)
                ParseManipulateValidateNode(manipulateNode, columnScrap);
            else if (manipulateNode.LocalName == ScrapXmlConsts.PurgeNodeName)
                ParseManipulatePurgeNode(manipulateNode, columnScrap);
            else if (manipulateNode.LocalName == ScrapXmlConsts.DbchangeNodeName)
                ParseManipulateDbchangeNode(manipulateNode, columnScrap);
            else if (manipulateNode.LocalName == ScrapXmlConsts.HtmlDecodeNodeName)
                ParseManipulateHtmlDecodeNode(manipulateNode, columnScrap);
            else
                throw new ScrapXmlException(ScrapXmlException.EErrorType.INVALID_MANIPULATE_CHILD_ITEM);
        }

        /// <summary>
        /// Parse the Manipulate SPlit node tag
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateSplitNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            SplitElement splitElement = xmlDocReader.ReadElement<SplitElement>(manipulateNode);
            splitElement.Data = Normalize(splitElement.Data);
            splitElement.Parent = columnScrap;
            columnScrap.Manipulation.Manipulations.Add(splitElement);
        }

        /// <summary>
        /// Parse the Manipulate Trim node tag
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateTrimNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            TrimElement trimElement = xmlDocReader.ReadElement<TrimElement>(manipulateNode);
            trimElement.Data = Normalize(trimElement.Data);
            trimElement.Parent = columnScrap;
            columnScrap.Manipulation.Manipulations.Add(trimElement);
        }

        /// <summary>
        /// Parse the Manipulate Regex node tag
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateRegexNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            RegexElement regexElement = xmlDocReader.ReadElement<RegexElement>(manipulateNode);
            regexElement.Pattern = Normalize(regexElement.Pattern);
            regexElement.Parent = columnScrap;
            columnScrap.Manipulation.Manipulations.Add(regexElement);
        }

        /// <summary>
        /// Parse the Manipulate Replace node tag
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateReplaceNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            ReplaceElement replaceElement = xmlDocReader.ReadElement<ReplaceElement>(manipulateNode);
            replaceElement.InString = Normalize(replaceElement.InString);
            replaceElement.OutString = Normalize(replaceElement.OutString);
            replaceElement.Parent = columnScrap;
            columnScrap.Manipulation.Manipulations.Add(replaceElement);
        }

        /// <summary>
        /// Parse the Manipulate Regex Replace node tag
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateRegexReplaceNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            RegexReplaceElement regexReplaceElement = xmlDocReader.ReadElement<RegexReplaceElement>(manipulateNode);
            regexReplaceElement.Pattern = Normalize(regexReplaceElement.Pattern);
            regexReplaceElement.Replace = Normalize(regexReplaceElement.Replace);
            regexReplaceElement.Parent = columnScrap;
            columnScrap.Manipulation.Manipulations.Add(regexReplaceElement);
        }

        /// <summary>
        /// Parse the Manipulate Validate node tag
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateValidateNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            ValidateElement validateElement = xmlDocReader.ReadElement<ValidateElement>(manipulateNode);
            validateElement.Parent = columnScrap;
            //columnScrap.Manipulation.Manipulations.Add(validateElement);
        }

        /// <summary>
        /// Parse the Manipulate Purge node tag
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulatePurgeNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            PurgeElement purgeElement = xmlDocReader.ReadElement<PurgeElement>(manipulateNode);
            purgeElement.Parent = columnScrap;
            columnScrap.Manipulation.Manipulations.Add(purgeElement);
        }

        /// <summary>
        /// Parse the HtmlDecode node
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateHtmlDecodeNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            HtmlDecodeElement htmlDecodeElement = xmlDocReader.ReadElement<HtmlDecodeElement>(manipulateNode);
            htmlDecodeElement.Parent = columnScrap;
            columnScrap.Manipulation.Manipulations.Add(htmlDecodeElement);
        }

        /// <summary>
        /// Parse the Manipulate Dbchange node tag
        /// </summary>
        /// <param name="manipulateNode"></param>
        /// <param name="columnScrap"></param>
        private void ParseManipulateDbchangeNode(XmlNode manipulateNode, ColumnElement columnScrap)
        {
            DbchangeElement dbchangeElement = xmlDocReader.ReadElement<DbchangeElement>(manipulateNode);
            dbchangeElement.Parent = columnScrap;

            ParseSelectElement(dbchangeElement, manipulateNode);
            columnScrap.Manipulation.Manipulations.Add(dbchangeElement);
        }

        /// <summary>
        /// Parse and process the Dbchange element
        /// </summary>
        /// <param name="dbchangeElement"></param>
        /// <param name="manipulateNode"></param>
        private void ParseSelectElement(DbchangeElement dbchangeElement, XmlNode manipulateNode)
        {
            if(manipulateNode.ChildNodes != null && manipulateNode.ChildNodes.Count == 1)
            {
                DbchangeSelectElement selectElement =
                     xmlDocReader.ReadElement<DbchangeSelectElement>(manipulateNode.ChildNodes[0]);
                selectElement.Parent = dbchangeElement;
                dbchangeElement.Select = selectElement;
            }
        }

        /// <summary>
        /// Standard html value normalization
        /// </summary>
        /// <param name="htmlValue"></param>
        /// <returns></returns>
        protected string Normalize(string htmlValue)
        {
            if (string.IsNullOrEmpty(htmlValue)) return htmlValue;
            htmlValue = HttpUtility.HtmlDecode(htmlValue);

            return htmlValue.Replace(ASCIICharacters.NewLineString, ASCIICharacters.NewLine)
                .Replace(ASCIICharacters.TabString, ASCIICharacters.Tab);
        }
    }
}
