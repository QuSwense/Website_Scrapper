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
        public static event Action<SegmentMetadataVisitorEventArgs> HandleVisitorData;

        public WebSegmentTree ParentWebTree { get; set; }
        public SegmentMetadataTree Parent { get; set; }

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

        public static void ResetEvent()
        {
            HandleVisitorData = null;
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
        /// A breadth first Travsersal implementation
        /// </summary>
        /// <param name="siblingIndx"></param>
        public void BFSTraversal(int siblingIndx)
        {
            HandleVisitorData?.Invoke(new SegmentMetadataVisitorEventArgs(this));

            // Child
            if (Nodes != null)
            {
                for (int indx = 0; indx < Nodes.Count; ++indx)
                {
                    SegmentMetadataTree metadataTree = Nodes[indx];
                    metadataTree.BFSTraversal(indx);
                }
            }
        }
    }
}
