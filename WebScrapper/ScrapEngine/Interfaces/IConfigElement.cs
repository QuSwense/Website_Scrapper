using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Interfaces
{
    /// <summary>
    /// A base class interface for all the Scrap config nodes
    /// </summary>
    public interface IConfigElement
    {
        /// <summary>
        /// Get the Id of the scrap config unit.
        /// If the current element is child element then find the id defined at the parent level 
        /// recursively
        /// </summary>
        string IdScrapUnit { get; }

        /// <summary>
        /// Get the Id of the scrap config element.
        /// It recursively calculates the level id and sibling id and appends to the main
        /// element
        /// </summary>
        string IdCurrent { get; }

        /// <summary>
        /// Get the name attribute defined in the element
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the level of the current node in the scrap unit tree
        /// </summary>
        int LevelInScrapUnit { get; }

        /// <summary>
        /// Get the sibling index of the current node under its parent
        /// </summary>
        int SiblingIndex { get; }

        /// <summary>
        /// Return the parent config element of the current config element
        /// </summary>
        IConfigElement ParentConfig { get; }

        /// <summary>
        /// A set of child elements in order of occurance
        /// </summary>
        List<IConfigElement> Children { get; set; }
    }
}
