using System.Collections.Generic;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// The class which temporarily maintains the states during the web scrapping and parsing
    /// activity
    /// </summary>
    public class ScrapIteratorStateModel
    {
        /// <summary>
        /// Stores all the states for the scrap nodes
        /// </summary>
        public Dictionary<string, ScrapIteratorArgs> WebScrapArgs { get; set; }

        /// <summary>
        /// This stores if the column metadata is updated in database for a Scrap unit id 
        /// </summary>
        public Dictionary<string, bool> ColumnMetadataUpdateFlags { get; set; }

        /// <summary>
        /// The current scrap iterator argumnets
        /// </summary>
        public ScrapIteratorArgs CurrentScrapIteratorArgs { get; protected set; }

        /// <summary>
        /// The current column iterator arguments
        /// </summary>
        public ColumnScrapIteratorArgs CurrentColumnScrapIteratorArgs { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapIteratorStateModel()
        {
            WebScrapArgs = new Dictionary<string, ScrapIteratorArgs>();
            ColumnMetadataUpdateFlags = new Dictionary<string, bool>();
        }

        /// <summary>
        /// Add the root scrap node arguments state
        /// </summary>
        /// <param name="scrapIteratorArgs"></param>
        public void AddRootScrapArgsState(ScrapIteratorArgs scrapIteratorArgs)
        {
            // If the Xml Node contains Id then this is the root Scrap node
            if (!WebScrapArgs.ContainsKey(scrapIteratorArgs.ScrapConfigObj.IdString))
                WebScrapArgs.Add(scrapIteratorArgs.ScrapConfigObj.IdString, scrapIteratorArgs);
            CurrentScrapIteratorArgs = scrapIteratorArgs;
        }

        /// <summary>
        /// Add the intermediate scrap iterator arguments
        /// </summary>
        /// <param name="scrapIteratorArgs"></param>
        public void AddNewScrap(ScrapIteratorArgs scrapIteratorArgs)
        {
            scrapIteratorArgs.Parent = CurrentScrapIteratorArgs;
            CurrentScrapIteratorArgs.Child.Add(scrapIteratorArgs);
            CurrentScrapIteratorArgs = scrapIteratorArgs;
        }

        /// <summary>
        /// When the currenr scrap iterator work is completed, restore or reset the current
        /// pointer to the parent
        /// </summary>
        public void RestoreScrap()
        {
            CurrentScrapIteratorArgs = CurrentScrapIteratorArgs.Parent;
        }

        /// <summary>
        /// Add the current column scrap iterator (as a row)
        /// </summary>
        /// <param name="columnScrapIteratorArgs"></param>
        public void AddNewColumn(ColumnScrapIteratorArgs columnScrapIteratorArgs)
        {
            CurrentScrapIteratorArgs.Columns.Add(columnScrapIteratorArgs);
            CurrentColumnScrapIteratorArgs = columnScrapIteratorArgs;
        }

        /// <summary>
        /// Set the current column configurator
        /// </summary>
        /// <param name="columnConfig"></param>
        public void SetColumnConfig(ColumnElement columnConfig)
        {
            CurrentScrapIteratorArgs.Columns[CurrentScrapIteratorArgs.Columns.Count - 1].ColumnElementConfig = columnConfig;
        }

        /// <summary>
        /// Reset the current column iterator
        /// </summary>
        public void RestoreColumn()
        {
            CurrentColumnScrapIteratorArgs = null;
        }

        /// <summary>
        /// Check if the column metadat is already upfdated in the database
        /// </summary>
        public bool IsColumnMetadataUpdated
        {
            get
            {
                if (!ColumnMetadataUpdateFlags.ContainsKey(CurrentColumnScrapIteratorArgs.Parent.ScrapConfigObj.Id)) return false;
                else
                {
                    return ColumnMetadataUpdateFlags[CurrentColumnScrapIteratorArgs.Parent.ScrapConfigObj.Id];
                }
            }
        }

        /// <summary>
        /// Set the column metdata flag
        /// </summary>
        public void SetColumnMetadataFlag()
        {
            ScrapElement scrapElement = CurrentColumnScrapIteratorArgs.Parent.ScrapConfigObj;
            if (!ColumnMetadataUpdateFlags.ContainsKey(scrapElement.Id))
            {
                ColumnMetadataUpdateFlags.Add(scrapElement.Id, true);
            }
            else
                ColumnMetadataUpdateFlags[scrapElement.Id] = true;
        }
    }
}
