using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using ScrapException;
using SqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The class to measure performance
    /// </summary>
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
        public Dictionary<string, PerformanceState> StorePerformances { get; set; }

        /// <summary>
        /// Store the list of performances by unique names
        /// </summary>
        public Dictionary<string, PerformanceStateModel> Performances { get; set; }

        /// <summary>
        /// The name of the current scrap node
        /// </summary>
        public string CurrentScrapNodeName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PerformanceMeasure()
        {
            StorePerformances = new Dictionary<string, PerformanceState>();
            Performances = new Dictionary<string, PerformanceStateModel>();
        }

        /// <summary>
        /// Get the key for the current config element in a loop with index
        /// </summary>
        /// <param name="configElement"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetKey(IConfigElement configElement, int index)
        {
            return string.Format("{0}_{1}", configElement.IdCurrent, index);
        }

        /// <summary>
        /// Start measuring performance for the element
        /// </summary>
        /// <param name="configElement"></param>
        public void Start(IConfigElement configElement, int index)
        {
            string key = GetKey(configElement, index);
            Debug.Assert(!StorePerformances.ContainsKey(key));

            PerformanceStateModel state = new PerformanceStateModel();
            state.Start();

            Performances.Add(key, state);
        }

        /// <summary>
        /// Start measuring performance for the element
        /// </summary>
        /// <param name="configElement"></param>
        public void Stop(IConfigElement configElement, int index)
        {
            string key = GetKey(configElement, index);
            Debug.Assert(StorePerformances.ContainsKey(key));

            Performances[key].Stop();
        }

        /// <summary>
        /// Add a new performance counter for the scrap node
        /// </summary>
        /// <param name="xmlNode"></param>
        public void NewChildNode(string name)
        {
            if (!StorePerformances.ContainsKey(name))
            {
                PerformanceState performanceState = new PerformanceState();
                StorePerformances.Add(name, performanceState);

                CurrentScrapNodeName = name;
            }

            stopwatchScrapElementProcessing = new Stopwatch();
            stopwatchScrapElementProcessing.Start();
        }

        /// <summary>
        /// The perfromance counter for the node to be completed
        /// </summary>
        public void FinalChildNode()
        {
            if (stopwatchScrapElementProcessing != null)
            {
                stopwatchScrapElementProcessing.Stop();
                StorePerformances[CurrentScrapNodeName].ScrapElapsedList.Add(
                    stopwatchScrapElementProcessing.Elapsed);
            }
        }

        /// <summary>
        /// Add a new url load
        /// </summary>
        /// <param name="url"></param>
        public void NewHtmlLoad(ScrapElement scrapElement)
        {
            stopwatchTemp = new Stopwatch();
            stopwatchTemp.Start();

            if (string.IsNullOrEmpty(CurrentScrapNodeName))
                CurrentScrapNodeName = scrapElement.Id;

            if (!StorePerformances.ContainsKey(CurrentScrapNodeName))
                StorePerformances.Add(CurrentScrapNodeName, new PerformanceState());
            StorePerformances[CurrentScrapNodeName].ElapsedHtmlLoads.Add(scrapElement.Url, TimeSpan.MinValue);
        }

        /// <summary>
        /// Time the load of url
        /// </summary>
        /// <param name="url"></param>
        public void FinalHtmlLoad(string url)
        {
            stopwatchTemp.Stop();
            StorePerformances[CurrentScrapNodeName].ElapsedHtmlLoads[url] = 
                stopwatchTemp.Elapsed;
        }

        /// <summary>
        /// Add a new url load
        /// </summary>
        /// <param name="url"></param>
        public void NewDbUpdate(List<List<DynamicTableDataInsertModel>> colValues, ColumnScrapStateModel scrapArgs)
        {
            stopwatchTemp = new Stopwatch();
            stopwatchTemp.Start();

            string id = scrapArgs.Parent.ScrapConfigObj.Id;

            StorePerformances[scrapArgs.Parent.ScrapConfigObj.Id].ElapsedDbUpdates
                .Add(scrapArgs.NodeIndexId, TimeSpan.MinValue);
        }

        /// <summary>
        /// Time the load of url
        /// </summary>
        /// <param name="url"></param>
        public void FinalDbUpdate(List<List<DynamicTableDataInsertModel>> colValues, ColumnScrapStateModel scrapArgs)
        {
            stopwatchTemp.Stop();
            //string key = GetColumnKey(colValues, keyIndex);

            StorePerformances[scrapArgs.Parent.ScrapConfigObj.Id].ElapsedDbUpdates[scrapArgs.NodeIndexId] =
                stopwatchTemp.Elapsed;
        }

        /// <summary>
        /// Get unique key to store performance data for a table
        /// </summary>
        /// <param name="colValues"></param>
        /// <param name="keyIndex"></param>
        /// <returns></returns>
        private string GetColumnKey(List<List<DynamicTableDataInsertModel>> colValues, string keyIndex)
        {
            return string.Join(",", colValues[0].Select(p => p.Name)) + keyIndex;
        }
    }
}
