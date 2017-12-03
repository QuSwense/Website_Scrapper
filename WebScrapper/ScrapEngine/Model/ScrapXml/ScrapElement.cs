using ScrapEngine.Common;
using System;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    public abstract class ScrapElement
    {
        #region Xml Attributes

        /// <summary>
        /// The id of the node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IdAttributeName)]
        public string IdString { get; set; }

        /// <summary>
        /// The name of the node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.NameAttributeName)]
        public string Name { get; set; }

        /// <summary>
        /// The url
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.UrlAttributeName)]
        public string UrlOriginal { get; set; }

        /// <summary>
        /// The type of the scrap node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.DoUpdateOnlyAttributeName)]
        public bool DoUpdateOnly { get; set; }

        #endregion Xml Attributes

        #region Calculated

        /// <summary>
        /// Get the level with respect to the parent Scrap node
        /// </summary>
        public int Level
        {
            get
            {
                int level = 0;
                ScrapElement tmpObj = this;
                while (tmpObj.Parent != null)
                {
                    tmpObj = tmpObj.Parent;
                    ++level;
                }

                return level;
            }
        }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName
        {
            get
            {
                ScrapElement tmpObj = this;

                while (tmpObj != null)
                {
                    if (!string.IsNullOrEmpty(tmpObj.Name))
                    {
                        return tmpObj.Name;
                    }

                    tmpObj = tmpObj.Parent;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// The id of the node
        /// </summary>
        public string Id
        {
            get
            {
                ScrapElement tmpObj = this;

                while (tmpObj != null)
                {
                    if (!string.IsNullOrEmpty(tmpObj.IdString))
                    {
                        return tmpObj.IdString;
                    }

                    tmpObj = tmpObj.Parent;
                }

                return string.Empty;
            }
        }

        public string UrlCalculated { get; set; }

        public string Url
        {
            get
            {
                if (!string.IsNullOrEmpty(UrlCalculated)) return UrlCalculated;
                else return UrlOriginal;
            }
        }

        #endregion Calculated

        /// <summary>
        /// The list of column nodes
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ColumnNodeName)]
        public List<ColumnElement> Columns { get; set; }

        /// <summary>
        /// The parent node
        /// </summary>
        public ScrapElement Parent { get; set; }

        /// <summary>
        /// The list of child Scrap element
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ScrapHtmlTableNodeName, typeof(ScrapHtmlTableElement))]
        [DXmlElement(ScrapXmlConsts.ScrapCsvNodeName, typeof(ScrapCsvElement))]
        public List<ScrapElement> Scraps { get; set; }

        public ScrapElement()
        {
            Scraps = new List<ScrapElement>();
            Columns = new List<ColumnElement>();
            DoUpdateOnly = false;
        }

        
    }
}
