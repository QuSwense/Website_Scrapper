using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Bl
{
    public class PerformanceMeasure
    {
        /// <summary>
        /// To measure the time of processing of Scrap element
        /// </summary>
        private Stopwatch stopwatchScrapElementProcessing;

        /// <summary>
        /// Stopwatch to time the html load
        /// </summary>
        private Stopwatch stopwatchTemp;

        /// <summary>
        /// Store the list of performances by unique names
        /// </summary>
        private Dictionary<string, PerformanceState> storePerformances;

        /// <summary>
        /// The name of the current scrap node
        /// </summary>
        private string CurrentScrapNodeName;

        /// <summary>
        /// Constructor
        /// </summary>
        public PerformanceMeasure()
        {
            storePerformances = new Dictionary<string, PerformanceState>();
        }

        /// <summary>
        /// Add a new performance counter for the scrap node
        /// </summary>
        /// <param name="xmlNode"></param>
        public void NewChildNode(string name)
        {
            if(!storePerformances.ContainsKey(name))
            {
                if(stopwatchScrapElementProcessing != null)
                {
                    stopwatchScrapElementProcessing.Stop();
                    storePerformances[CurrentScrapNodeName].TotalElapsed =
                        stopwatchScrapElementProcessing.Elapsed;
                }

                PerformanceState performanceState = new PerformanceState();
                storePerformances.Add(name, performanceState);
                stopwatchScrapElementProcessing = new Stopwatch();
                stopwatchScrapElementProcessing.Start();

                CurrentScrapNodeName = name;
            }
        }

        /// <summary>
        /// Add a new url load
        /// </summary>
        /// <param name="url"></param>
        public void NewHtmlLoad(string url)
        {
            stopwatchTemp = new Stopwatch();
            stopwatchTemp.Start();

            storePerformances[CurrentScrapNodeName].ElapsedHtmlLoads.Add(url, TimeSpan.MinValue);
        }

        /// <summary>
        /// Time the load of url
        /// </summary>
        /// <param name="url"></param>
        public void FinalHtmlLoad(string url)
        {
            stopwatchTemp.Stop();
            storePerformances[CurrentScrapNodeName].ElapsedHtmlLoads[url] = 
                stopwatchTemp.Elapsed;
        }

        /// <summary>
        /// Add a new url load
        /// </summary>
        /// <param name="url"></param>
        public void NewDbUpdate(string keyIndex)
        {
            stopwatchTemp = new Stopwatch();
            stopwatchTemp.Start();

            storePerformances[CurrentScrapNodeName].ElapsedHtmlLoads.Add(keyIndex, TimeSpan.MinValue);
        }

        /// <summary>
        /// Time the load of url
        /// </summary>
        /// <param name="url"></param>
        public void FinalDbUpdate(string keyIndex)
        {
            stopwatchTemp.Stop();
            storePerformances[CurrentScrapNodeName].ElapsedHtmlLoads[keyIndex] =
                stopwatchTemp.Elapsed;
        }
    }
}
