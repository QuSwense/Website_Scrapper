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
        /// The manipulate tag
        /// </summary>
        public List<ManipulateElement> Manipulations { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ColumnElement()
        {
            Manipulations = new List<ManipulateElement>();
        }
    }
}
