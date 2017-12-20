using SqliteDatabase.Model;
using System.Collections.Generic;
using System;

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
        /// Store the row data for one single loop of iteration for each level of data scrapping
        /// </summary>
        public List<List<DynamicTableDataInsertModel>> RowDataInCurrentLoop { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapIteratorStateModel()
        {
            WebScrapArgs = new Dictionary<string, ScrapIteratorArgs>();
            ColumnMetadataUpdateFlags = new Dictionary<string, bool>();
            RowDataInCurrentLoop = new List<List<DynamicTableDataInsertModel>>();
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
        /// Add column data
        /// </summary>
        /// <param name="columnData"></param>
        /// <param name="colElement"></param>
        public void AddColumnData(List<DynamicTableDataInsertModel> columnData, ColumnElement colElement)
        {
            foreach (var row in RowDataInCurrentLoop)
            {
                foreach (var col in columnData)
                {
                    row.Add(col);
                }
            }
        }

        /// <summary>
        /// Add column data
        /// </summary>
        /// <param name="columnData"></param>
        /// <param name="colElement"></param>
        public void AddColumnData(List<List<DynamicTableDataInsertModel>> columnData, ColumnElement colElement)
        {
            foreach (var row in RowDataInCurrentLoop)
            {
                foreach (var col in columnData)
                {
                    row.AddRange(col);
                }
            }
        }

        /// <summary>
        /// Check if the column scrapped data is already available
        /// </summary>
        /// <param name="colElement"></param>
        /// <returns></returns>
        public bool IsColumnDataAvailable(ColumnElement colElement)
        {
            if (colElement.Level != -1 && colElement.Level < colElement.Parent.Level)
                if (RowDataInCurrentLoop.ContainsKey(colElement.Parent.Level))
                    return true;
            return false;
        }

        /// <summary>
        /// Get column data list
        /// </summary>
        /// <returns></returns>
        public List<List<DynamicTableDataInsertModel>> GetColumnDataList()
        {
            var rows = new List<List<DynamicTableDataInsertModel>>();

            foreach (var kvcols in RowDataInCurrentLoop)
            {
                foreach (var col in kvcols.Value)
                {
                    rows.Add(col);
                }
            }

            return rows;
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
