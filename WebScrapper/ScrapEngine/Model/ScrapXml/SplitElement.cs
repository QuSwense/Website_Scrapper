using ScrapEngine.Interfaces;
using ScrapEngine.Model.ScrapXml;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The class for manipulating a scrapped data
    /// The element is used to manipulate the data by splitting the scrapped data
    /// and picking a split element by index.
    /// </summary>
    public class SplitElement : ManipulateChildElement
    {
        /// <summary>
        /// The split string by which the scrapped data needs to be splitted
        /// </summary>
        [DXmlAttribute("data", IsMandatory = true)]
        public string Data { get; set; }

        /// <summary>
        /// The index of the split array that we need as data
        /// </summary>
        [DXmlAttribute("index", IsMandatory = true)]
        public int Index { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SplitElement()
        {
            manipulateType = EManipulateType.Split;
        }
    }
}
