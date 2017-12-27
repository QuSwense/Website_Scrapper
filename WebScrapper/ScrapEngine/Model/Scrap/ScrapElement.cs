using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using ScrapEngine.Model.Scrap;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// Base class for any Scrap type element
    /// </summary>
    public abstract class ScrapElement : ConfigElementBase
    {
        #region Xml Attributes

        /// <summary>
        /// The id of the node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IdAttributeName)]
        public string IdString { get; set; }
        
        /// <summary>
        /// The url value present in the xml config
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.UrlAttributeName)]
        public string UrlOriginal { get; set; }

        /// <summary>
        /// The type of the scrap node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.DoUpdateOnlyAttributeName,
            ConfigElementConsts.DoUpdateOnlyColumnDefault)]
        public bool DoUpdateOnly { get; set; }

        #endregion Xml Attributes

        #region Calculated

        /// <summary>
        /// Get the root Scrap element tag
        /// </summary>
        public IConfigElement ScrapRoot
        {
            get
            {
                IConfigElement scrapRoot = this;
                while (scrapRoot.ParentConfig != null) scrapRoot = scrapRoot.ParentConfig;
                return scrapRoot;
            }
        }

        /// <summary>
        /// Get The name of the table defined at any level. It returns the
        /// first name encountered in the scrap
        /// </summary>
        public string TableName
        {
            get { return ScrapRoot.Name; }
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

        #region IConfigElement Implementation

        /// <summary>
        /// Get the Id of the scrap config unit.
        /// If the current element is child element then find the id defined at the parent level 
        /// recursively
        /// </summary>
        public override string IdScrapUnit
        {
            get
            {
                if (ScrapParent == null) return IdString;
                else return ScrapParent.IdScrapUnit;
            }
        }

        /// <summary>
        /// A set of child elements in order of occurance
        /// </summary>
        [DXmlElement(ScrapXmlConsts.DbRowNodeName)]
        [DXmlElement(ScrapXmlConsts.ScrapHtmlTableNodeName, typeof(ScrapHtmlTableElement))]
        [DXmlElement(ScrapXmlConsts.ScrapCsvNodeName, typeof(ScrapCsvElement))]
        public override List<IConfigElement> Children { get; set; }

        #endregion IConfigElement Implementation
    }
}
