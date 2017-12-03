using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using WebCommon.Error;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// The base class for Manipulate child items
    /// This class is required to store the type of Child node information
    /// </summary>
    public class ManipulateChildElement : IColumnChild
    {
        #region References

        /// <summary>
        /// Points to the parent scrap node
        /// </summary>
        public ColumnElement Parent { get; set; }

        #endregion References

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ManipulateChildElement() { }

        #endregion Constructor
    }
}
