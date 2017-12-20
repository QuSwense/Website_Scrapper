using ScrapEngine.Common;
using ScrapEngine.Model.Scrap;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// Base class for any Scrap type element
    /// </summary>
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
        /// The url value present in the xml config
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
        /// Get the level of the node wrt the parent scrap nodes.
        /// It does not get the actual level of the node in the xml config.
        /// It gets the level of the Scrap node under other scrap nodes.
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
        /// Get The name of the table defined at any level. It returns the
        /// first name encountered in the scrap
        /// </summary>
        public string TableName
        {
            get
            {
                ScrapElement tmpObj = this;
                while (tmpObj != null)
                {
                    if (!string.IsNullOrEmpty(tmpObj.Name)) return tmpObj.Name;
                    tmpObj = tmpObj.Parent;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// The id of the scrap nodes. There is only one id attribute defined at
        /// the parent level of a scrap node
        /// </summary>
        public string Id
        {
            get
            {
                ScrapElement tmpObj = this;
                while (tmpObj != null)
                {
                    if (!string.IsNullOrEmpty(tmpObj.IdString)) return tmpObj.IdString;
                    tmpObj = tmpObj.Parent;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Stores the actual value of the Url. The original url contains the value stored in 
        /// xml config file which might have parameters.
        /// </summary>
        public string UrlCalculated { get; set; }

        /// <summary>
        /// Get the Url value
        /// </summary>
        public string Url
        {
            get
            {
                return (!string.IsNullOrEmpty(UrlCalculated))? UrlCalculated: UrlOriginal;
            }
        }

        #endregion Calculated

        #region References

        /// <summary>
        /// The list of column nodes
        /// </summary>
        [DXmlElement(ScrapXmlConsts.DbRowNodeName)]
        public DbRowElement DbRow { get; set; }

        /// <summary>
        /// The parent node
        /// </summary>
        [DXmlParent]
        public ScrapElement Parent { get; set; }

        /// <summary>
        /// The list of child Scrap element
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ScrapHtmlTableNodeName, typeof(ScrapHtmlTableElement))]
        [DXmlElement(ScrapXmlConsts.ScrapCsvNodeName, typeof(ScrapCsvElement))]
        public List<ScrapElement> Scraps { get; set; }

        #endregion References

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScrapElement()
        {
            DoUpdateOnly = false;
        }

        #endregion Constructor
    }
}
