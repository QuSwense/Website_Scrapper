using ScrapEngine.Interfaces;
using ScrapEngine.Model.ScrapXml;
using System.Collections.Generic;

namespace ScrapEngine.Model
{
    /// <summary>
    /// This class represents the "Manipulate" element tag of the Scrap Config xml.
    /// This element defines different child elements to manipulate the scrapped data.
    /// </summary>
    public class ManipulateElement
    {
        /// <summary>
        /// The name of the Element tag
        /// </summary>
        public static string TagName = "Manipulate";

        /// <summary>
        /// The list of manipulate Items tags. The order of the manipulation tags
        /// are important
        /// </summary>
        public List<ManipulateChildElement> ManipulateItems { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ManipulateElement()
        {
            ManipulateItems = new List<ManipulateChildElement>();
        }
    }
}
