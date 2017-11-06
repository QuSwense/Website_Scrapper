using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The class for parsing an reading a line of csv file.
    /// The element is used to manipulate the data
    /// </summary>
    public class WebDataConfigSplit
    {
        /// <summary>
        /// The data node
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// The index of the split array
        /// </summary>
        public int Index { get; set; }
    }
}
