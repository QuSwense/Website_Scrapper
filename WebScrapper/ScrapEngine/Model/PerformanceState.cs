using System;
using System.Collections.Generic;

namespace ScrapEngine.Model
{
    /// <summary>
    /// Performance state data
    /// </summary>
    public class PerformanceState
    {
        /// <summary>
        /// Calculate total elapsed time for a single loop WebData child node
        /// </summary>
        public List<TimeSpan> ScrapElapsedList { get; set; }

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
            ScrapElapsedList = new List<TimeSpan>();
            ElapsedHtmlLoads = new Dictionary<string, TimeSpan>();
            ElapsedDbUpdates = new Dictionary<string, TimeSpan>();
        }
    }
}
