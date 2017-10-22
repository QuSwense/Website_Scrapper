using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebReader.Csv
{
    /// <summary>
    /// This attribute is used with the csv reader
    /// </summary>
    public class SplitIndexAttribute : Attribute
    {
        /// <summary>
        /// The index of the csv column split
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        public SplitIndexAttribute(int index)
        {
            Index = index;
        }
    }
}
