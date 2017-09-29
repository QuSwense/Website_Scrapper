using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.OModel
{
    /// <summary>
    /// This class stores the processed and parsed data from the Website scrapper grammer
    /// </summary>
    public class ScrapWebData
    {
        /// <summary>
        /// The unique identifier of the node
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The parsed copyright text (This is a generic tree structure)
        /// </summary>
        public List<ScrapMetadata> Copyrights { get; set; }

        /// <summary>
        /// The parsed references text (This is a generic tree structure)
        /// </summary>
        public List<ScrapMetadata> References { get; set; }

        /// <summary>
        /// The child nodes. The data structure resembles a table like structure.
        /// A <see cref="List<ScrapWebData>"/> is a single row.
        /// </summary>
        public List<List<ScrapWebData>> Nodes { get; set; }

        /// <summary>
        /// The actual data of the scrapped website
        /// </summary>
        public ScrapMetadata Text { get; set; }
    }
}
