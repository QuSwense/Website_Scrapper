using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.Model
{
    public class SegmentMetadataVisitorEventArgs : EventArgs
    {
        public SegmentMetadataTree SegmentMetadata { get; protected set; }

        public SegmentMetadataVisitorEventArgs(SegmentMetadataTree segmentMetadata)
        {
            SegmentMetadata = segmentMetadata;
        }
    }
}
