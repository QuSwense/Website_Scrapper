using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Model
{
    /// <summary>
    /// Performance state data
    /// </summary>
    public class PerformanceState
    {
        /// <summary>
        /// Calculate total elapsed time for a WebData child node
        /// </summary>
        public TimeSpan TotalElapsed { get; set; }

        /// <summary>
        /// Elapsed time span for loading the Html page
        /// </summary>
        public Dictionary<string, TimeSpan> ElapsedHtmlLoads { get; set; }

        /// <summary>
        /// Elapsed time span for insert/update data in tables
        /// </summary>
        public Dictionary<string, TimeSpan> ElapsedDbUpdates { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PerformanceState()
        {
            ElapsedHtmlLoads = new Dictionary<string, TimeSpan>();
            ElapsedDbUpdates = new Dictionary<string, TimeSpan>();
        }
    }
}
