using ScrapEngine.Interfaces;
using ScrapEngine.Model.ScrapXml;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// Manipulate node data
    /// </summary>
    public class TrimElement : ManipulateChildElement
    {
        /// <summary>
        /// The data
        /// </summary>
        [DXmlAttribute("data", IsMandatory = true)]
        public string Data { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TrimElement()
        {
            manipulateType = EManipulateType.Trim;
        }
    }
}
