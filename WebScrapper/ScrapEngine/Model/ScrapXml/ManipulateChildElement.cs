using ScrapEngine.Common;
using WebCommon.Error;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// The base class for Manipulate child items
    /// This class is required to store the type of Child node information
    /// </summary>
    public class ManipulateChildElement
    {
        /// <summary>
        /// Points to the parent scrap node
        /// </summary>
        public ColumnElement Parent { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ManipulateChildElement() { }
    }
}
