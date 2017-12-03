using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Model.Parser
{
    public class ScrapIteratorStateModel
    {
        /// <summary>
        /// Stores all the states for the scrap nodes
        /// </summary>
        public Dictionary<string, ScrapIteratorArgs> WebScrapArgs { get; set; }

        public Dictionary<string, bool> ColumnMetadataUpdateFlags { get; set; }

        public ScrapIteratorArgs CurrentScrapIteratorArgs { get; protected set; }
        public ColumnScrapIteratorArgs CurrentColumnScrapIteratorArgs { get; protected set; }

        public ScrapIteratorStateModel()
        {
            WebScrapArgs = new Dictionary<string, ScrapIteratorArgs>();
            ColumnMetadataUpdateFlags = new Dictionary<string, bool>();
        }

        public void AddRootScrapArgsState(ScrapIteratorArgs scrapIteratorArgs)
        {
            // If the Xml Node contains Id then this is the root Scrap node
            if (!WebScrapArgs.ContainsKey(scrapIteratorArgs.ScrapConfigObj.IdString))
                WebScrapArgs.Add(scrapIteratorArgs.ScrapConfigObj.IdString, scrapIteratorArgs);
            CurrentScrapIteratorArgs = scrapIteratorArgs;
        }

        public void AddNewScrap(ScrapIteratorArgs scrapIteratorArgs)
        {
            scrapIteratorArgs.Parent = CurrentScrapIteratorArgs;
            CurrentScrapIteratorArgs.Child.Add(scrapIteratorArgs);
            CurrentScrapIteratorArgs = scrapIteratorArgs;
        }

        public void RestoreScrap()
        {
            CurrentScrapIteratorArgs = CurrentScrapIteratorArgs.Parent;
        }

        public void AddNewColumn(ColumnScrapIteratorArgs columnScrapIteratorArgs)
        {
            CurrentScrapIteratorArgs.Columns.Add(columnScrapIteratorArgs);
            CurrentColumnScrapIteratorArgs = columnScrapIteratorArgs;
        }

        public void RestoreColumn()
        {
            CurrentColumnScrapIteratorArgs = null;
        }

        public bool IsColumnMetadataUpdated
        {
            get
            {
                if (!ColumnMetadataUpdateFlags.ContainsKey(CurrentColumnScrapIteratorArgs.Parent.Id)) return false;
                else
                {
                    return ColumnMetadataUpdateFlags[CurrentColumnScrapIteratorArgs.Parent.Id];
                }
            }
        }

        public void SetColumnMetadataFlag()
        {
            if (!ColumnMetadataUpdateFlags.ContainsKey(CurrentColumnScrapIteratorArgs.Parent.Id))
            {
                ColumnMetadataUpdateFlags.Add(CurrentColumnScrapIteratorArgs.Parent.Id, true);
            }
            else
                ColumnMetadataUpdateFlags[CurrentColumnScrapIteratorArgs.Parent.Id] = true;
        }
    }
}
