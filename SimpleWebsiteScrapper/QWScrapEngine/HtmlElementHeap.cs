using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWScrapEngine
{
    /// <summary>
    /// An internal class to save the state of the html document that is currently been processed.
    /// It saves the current type of node which is either a single node, a collection or a attribute.
    /// This is used by the engine to store the current html link subtree.
    /// </summary>
    internal class HtmlElementHeap
    {
        /// <summary>
        /// The node collection object
        /// </summary>
        public HtmlNodeCollection Collection;

        /// <summary>
        /// Refers to a single node
        /// </summary>
        public HtmlNode Single;

        /// <summary>
        /// The attribute node
        /// </summary>
        public HtmlAttribute Attribute;

        /// <summary>
        /// The index for the collection node object <see cref="Collection"/>.
        /// If the value is less than 0, then it is a collection object, else it refers to a 
        /// single node
        /// </summary>
        public int CollectionIndex;

        /// <summary>
        /// Default constructor
        /// </summary>
        public HtmlElementHeap()
        {
            CollectionIndex = -1;
        }

        /// <summary>
        /// COnstructor with single node
        /// </summary>
        /// <param name="node"></param>
        public HtmlElementHeap(HtmlNode node)
        {
            Single = node;
        }
    }
}
