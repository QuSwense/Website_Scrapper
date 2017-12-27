using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Model
{
    public class PerformanceStateModel
    {
        /// <summary>
        /// To measure the time of processing of a config element
        /// </summary>
        private Stopwatch stopwatchProcessing;

        /// <summary>
        /// The final measure
        /// </summary>
        public TimeSpan Result { get; set; }

        /// <summary>
        /// Start timing the performance for the node
        /// </summary>
        public void Start()
        {
            stopwatchProcessing = new Stopwatch();
            stopwatchProcessing.Start();
        }

        /// <summary>
        /// Stop timing and register result
        /// </summary>
        public void Stop()
        {
            stopwatchProcessing.Stop();
            Result = stopwatchProcessing.Elapsed;
            stopwatchProcessing = null;
        }
    }
}
