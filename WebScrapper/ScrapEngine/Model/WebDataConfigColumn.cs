using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    /// <summary>
    /// REafs to a column of the table to store the data scrapped
    /// </summary>
    public class WebDataConfigColumn
    {
        /// <summary>
        /// Points to the parent scrap node
        /// </summary>
        public WebDataConfigScrap Parent { get; set; }
        
        /// <summary>
        /// The name of the column
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Whether this column is the unique key for insertion or updation
        /// </summary>
        public bool IsPk { get; set; }

        /// <summary>
        /// The index element in case of csv
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The xpath
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// The manipulate tag
        /// </summary>
        public WebDataConfigManipulate[] Manipulations { get; set; }

        public XmlNode State { get; set; }
    }
}
