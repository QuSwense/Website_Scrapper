using System;
using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// REafs to a column of the table to store the data scrapped
    /// </summary>
    public class ColumnElement
    {
        /// <summary>
        /// The name of the Element tag
        /// </summary>
        public static string TagName = "Column";

        /// <summary>
        /// Points to the parent scrap node
        /// </summary>
        public ScrapElement Parent { get; set; }

        /// <summary>
        /// The name of the column
        /// </summary>
        [DXmlAttribute("name", IsMandatory = true)]
        public string Name { get; set; }

        /// <summary>
        /// Whether this column is the unique key for insertion or updation
        /// </summary>
        [DXmlAttribute("isunique")]
        public bool IsUnique { get; set; }

        /// <summary>
        /// The index element in case of csv
        /// </summary>
        [DXmlAttribute("index")]
        public int Index { get; set; }

        /// <summary>
        /// The xpath
        /// </summary>
        [DXmlAttribute("xpath")]
        public string XPath { get; set; }

        /// <summary>
        /// Defines the cardinality of a relationship with other columns
        /// If a column is defined to be greater than 1 then it can have multiple values for combinations with other columns
        /// e.g, (col, col21), (col1, col22), etc. for col2.Cardinality = 2
        /// To represent 'n' cardinality use '*' in xml and as value we will use '-1'
        /// </summary>
        [DXmlAttribute("cardinal")]
        public string CardinalString { get; set; }

        /// <summary>
        /// The xpath
        /// </summary>
        [DXmlAttribute("level")]
        public int Level { get; set; }

        public int Cardinal
        {
            get
            {
                if (CardinalString == "*") return -1;
                else if(string.IsNullOrEmpty(CardinalString)) return 0;
                return Convert.ToInt32(CardinalString);
            }
        }

        /// <summary>
        /// The manipulate tag
        /// </summary>
        public List<ManipulateElement> Manipulations { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ColumnElement()
        {
            Manipulations = new List<ManipulateElement>();
            CardinalString = "1";
            Level = Int32.MinValue;
        }
    }
}
