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

        public ScrapIteratorArgs CurrentScrapIteratorArgs { get; protected set; }
        public ColumnScrapIteratorArgs CurrentColumnScrapIteratorArgs { get; protected set; }

        public ScrapIteratorStateModel()
        {
            WebScrapArgs = new Dictionary<string, ScrapIteratorArgs>();
        }

        public void AddRootScrapArgsState(ScrapIteratorArgs scrapIteratorArgs)
        {
            // If the Xml Node contains Id then this is the root Scrap node
            if (!WebScrapArgs.ContainsKey(scrapIteratorArgs.ScrapConfigObj.IdString))
                WebScrapArgs.Add(scrapIteratorArgs.ScrapConfigObj.IdString, scrapIteratorArgs);
            CurrentScrapIteratorArgs = scrapIteratorArgs;
        }

        public void AddNewScrap(ScrapIteratorHtmlArgs childScrapIteratorHtmlArgs)
        {
            CurrentScrapIteratorArgs.Child.Add(childScrapIteratorHtmlArgs);
            CurrentScrapIteratorArgs = childScrapIteratorHtmlArgs;
        }

        public void AddNewColumn(ColumnScrapIteratorArgs columnScrapIteratorArgs)
        {
            CurrentScrapIteratorArgs.Columns.Add(columnScrapIteratorArgs);
            CurrentColumnScrapIteratorArgs = columnScrapIteratorArgs;
        }
    }
}
