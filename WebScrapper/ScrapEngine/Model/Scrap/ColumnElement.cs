using ScrapEngine.Common;
using ScrapEngine.Model.Scrap;
using System;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The class for a column of the table to store the data scrapped
    /// </summary>
    public class ColumnElement
    {
        #region Xml Attributes

        /// <summary>
        /// The name of the column
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.NameAttributeName, IsMandatory = true)]
        public string Name { get; set; }

        /// <summary>
        /// Whether this column is the unique key for insertion or updation
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsUniqueAttributeName)]
        public bool IsUnique { get; set; }

        /// <summary>
        /// The index element in case of csv
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IndexAttributeName)]
        public int Index { get; set; }

        /// <summary>
        /// The xpath
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.XPathAttributeName)]
        public string XPath { get; set; }

        /// <summary>
        /// The xpath
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.ValueAsInnerHtmlAttributeName)]
        public bool ValueAsInnerHtml { get; set; }

        [DXmlAttribute(ScrapXmlConsts.SkipIfValueAttributeName)]
        public string SkipIfValueString { get; set; }

        /// <summary>
        /// Defines the cardinality of a relationship with other columns
        /// If a column is defined to be greater than 1 then it can have multiple values for combinations with other columns
        /// e.g, (col, col21), (col1, col22), etc. for col2.Cardinality = 2
        /// To represent 'n' cardinality use '*' in xml and as value we will use '-1'
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.CardinalAttributeName)]
        public string CardinalString { get; set; }

        /// <summary>
        /// States the level of the scrap node to pick the value from
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.LevelAttributeName)]
        public int Level { get; set; }

        [DXmlAttribute(ScrapXmlConsts.IndexAttributeName)]
        public int ColumnIndex { get; set; }

        #endregion Xml Attributes

        #region Calculated

        /// <summary>
        /// A list of values which if matches with the scrapped data then skip the whole row and do not store
        /// </summary>
        public List<string> SkipIfValues { get; set; }

        /// <summary>
        /// The integer value for the Cardinal type
        /// </summary>
        public int Cardinal
        {
            get
            {
                if (CardinalString == ScrapXmlConsts.AllValue) return -1;
                else if(string.IsNullOrEmpty(CardinalString)) return 0;
                return Convert.ToInt32(CardinalString);
            }
        }

        #endregion Calculated

        #region References
        
        /// <summary>
        /// The manipulate tag
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ManipulateNodeName)]
        public ManipulateElement Manipulation { get; set; }

        /// <summary>
        /// The validation tag to validate the column data just scrapped
        /// It is generally better to use it after all manipulations are done.
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ValidateNodeName)]
        public ValidateElement Validate { get; set; }

        /// <summary>
        /// The parent node
        /// </summary>
        [DXmlParent]
        public ScrapElement Parent { get; set; }

        #endregion References

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ColumnElement()
        {
            CardinalString = "1";
            Index = -1;
            Level = -1;
            ColumnIndex = -1;
            IsUnique = false;
        }

        #endregion Constructor
    }
}
