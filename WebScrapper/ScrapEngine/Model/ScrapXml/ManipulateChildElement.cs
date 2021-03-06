﻿using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// The base class for Manipulate child items
    /// This class is required to store the type of Child node information
    /// </summary>
    public class ManipulateChildElement
    {
        #region References

        /// <summary>
        /// Points to the parent scrap node
        /// </summary>
        [DXmlParent]
        public ManipulateElement Parent { get; set; }

        #endregion References
    }
}
