using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QWCommonDST.Helper;

namespace QWWebScrap.Model
{
    /// <summary>
    /// A simple Tree structure used by <see cref="SegmentMetadata"/> class
    /// The data structure assumes that every node is a subtree. A leaf node contains NULL subtree.
    /// </summary>
    public class SegmentMetadataTree
    {
        /// <summary>
        /// Refers to the Parent Main Webtree node
        /// </summary>
        public WebSegmentTree ParentWebTree { get; set; }

        /// <summary>
        /// Refers to the parent node
        /// </summary>
        public SegmentMetadataTree Parent { get; set; }

        /// <summary>
        /// The unique id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// List of children
        /// </summary>
        public List<SegmentMetadataTree> Nodes { get; set; }

        /// <summary>
        /// The actual data content of the Tree node
        /// </summary>
        public SegmentMetadata ActualData { get; set; }

        /// <summary>
        /// Check if this current node is a leaf node or not
        /// </summary>
        public bool IsLeaf { get { return Nodes.IsNullOrEmpty(); } }
        
        /// <summary>
        /// Add a new node
        /// </summary>
        /// <param name="metadataTree"></param>
        public SegmentMetadataTree AddChild()
        {
            if (Nodes == null) Nodes = new List<SegmentMetadataTree>();
            SegmentMetadataTree metadataTree = new SegmentMetadataTree();
            metadataTree.Parent = this;
            Nodes.Add(metadataTree);
            return metadataTree;
        }

        /// <summary>
        /// Add data node
        /// </summary>
        /// <returns></returns>
        public SegmentMetadata AddData()
        {
            if (ActualData == null) ActualData = new SegmentMetadata();
            return ActualData;
        }

        /// <summary>
        /// Add xpath data to reference
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="collIndex"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public SegmentMetadataTree AddChildPath(string xpath, int collIndex = -1, string attribute = null)
        {
            if (Nodes == null) Nodes = new List<SegmentMetadataTree>();
            SegmentMetadataTree node = AddChild();
            node.AddData().AddPath(xpath, collIndex, attribute);
            return node;
        }
    }
}
