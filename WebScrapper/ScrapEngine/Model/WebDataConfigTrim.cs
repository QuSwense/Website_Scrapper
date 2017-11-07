using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// Manipulate node data
    /// </summary>
    public class WebDataConfigTrim
    {
        /// <summary>
        /// The data
        /// </summary>
        [DXmlAttribute("data", IsMandatory = true)]
        public string Data { get; set; }
    }
}
