using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using ScrapEngine.Model.Scrap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// This represents the 'Column' element in the scrap config xml file.
    /// The column element identifies the column of a table and has parameters
    /// to fetch data from the website.
    /// </summary>
    public class ColumnElement : ConfigElementBase
    {
        #region Xml Attributes

        /// <summary>
        /// Whether this column is the unique key for insertion or updation
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsUniqueAttributeName, 
            ConfigElementConsts.IsUniqueColumnDefault)]
        public bool IsUnique { get; set; }

        /// <summary>
        /// The index element in case of csv
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IndexAttributeName,
            ConfigElementConsts.IndexColumnDefault)]
        public int Index { get; set; }

        /// <summary>
        /// The xpath which is used to extract the data from the web navigator
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.XPathAttributeName,
            IsMandatory = true)]
        public string XPath { get; set; }

        /// <summary>
        /// This flag specifies whether the whole Inner Html context is to be scrapped and not 
        /// simply the value
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.ValueAsInnerHtmlAttributeName,
            ConfigElementConsts.ValueAsInnerHtmlColumnDefault)]
        public bool ValueAsInnerHtml { get; set; }

        #endregion Xml Attributes

        #region IConfigElement Implementation

        /// <summary>
        /// A set of child elements in order of occurance
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ManipulateNodeName)]
        public override List<IConfigElement> Children { get; set; }

        #endregion IConfigElement Implementation
    }
}
