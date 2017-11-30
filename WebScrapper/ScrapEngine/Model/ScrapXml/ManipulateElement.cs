﻿using ScrapEngine.Model.ScrapXml;
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
