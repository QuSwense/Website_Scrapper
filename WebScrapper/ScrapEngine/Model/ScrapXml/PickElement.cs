using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// This class is used to pick mulitple values in case of multiple cardinality
    /// </summary>
    public class PickElement
    {
        /// <summary>
        /// Pick the data from the parent value using index.
        /// if the index is -1 pick all in the array / list
        /// </summary>
        public int Index { get; set; }

        public PickElement()
        {
            Index = 0;
        }
    }
}
