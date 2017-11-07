using WebReader.Model;

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
        [DXmlAttribute("data", IsMandatory = true)]
        public string Data { get; set; }

        /// <summary>
        /// The index of the split array
        /// </summary>
        [DXmlAttribute("index", IsMandatory = true)]
        public int Index { get; set; }
    }
}
