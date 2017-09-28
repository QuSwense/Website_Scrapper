using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.Model
{
    public class WebSegmentVisitorEventArgs: EventArgs
    {
        public WebSegmentTree Segment { get; protected set; }

        public WebSegmentVisitorEventArgs(WebSegmentTree segment)
        {
            Segment = segment;
        }
    }
}
