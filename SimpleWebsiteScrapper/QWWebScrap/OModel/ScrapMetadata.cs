using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.OModel
{
    /// <summary>
    /// The actual data class of the scrapped data
    /// </summary>
    public class ScrapMetadata
    {
        /// <summary>
        /// The unique Identifier of the data node
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The actual string data of the class
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The child nodes represented as a table like structure.
        /// </summary>
        public List<List<ScrapMetadata>> Nodes { get; set; }
    }
}
