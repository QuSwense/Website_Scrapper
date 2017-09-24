using System.Collections.Generic;

namespace SimpleWebsiteScrapper.DataSet
{
    /// <summary>
    /// The class which contains the actual data scrapped from the website alongside the metadata.
    /// </summary>
    public class WebpageSectionData
    {
        /// <summary>
        /// The list of copyright information
        /// </summary>
        public List<ProcessData> Copyrights { get; set; }

        /// <summary>
        /// The list of references
        /// </summary>
        public List<ProcessData> References { get; set; }

        /// <summary>
        /// The actual data (mostly is a string type)
        /// </summary>
        public ProcessData ClassData { get; set; }

        /// <summary>
        /// The list of children nodes
        /// </summary>
        public List<List<WebpageSectionData>> Nodes { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebpageSectionData()
        {
            Copyrights = new List<ProcessData>();
            References = new List<ProcessData>();
            ClassData = new ProcessData();
            Nodes = new List<List<WebpageSectionData>>();
        }
    }
}
