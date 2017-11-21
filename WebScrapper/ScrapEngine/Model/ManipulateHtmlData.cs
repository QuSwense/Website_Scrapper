using System.Collections.Generic;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The web scrapped data is Pushed through the Manipulation Xml processing tags
    /// </summary>
    public class ManipulateHtmlData
    {
        /// <summary>
        /// Stores the final XPath that is used to Scrapped the ata from the website
        ///  for the Html table
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// The url from where the data is scrapped
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Store the cardinality of the column to be scrapped
        /// </summary>
        public int Cardinality { get; set; }

        /// <summary>
        /// A temporary scrapped value before manipulation
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// Stores the values manipulated
        /// </summary>
        public List<string> Results { get; set; }

        public ManipulateHtmlData()
        {
            Results = new List<string>();
        }
    }
}
