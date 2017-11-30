using HtmlAgilityPack;
using System.Collections.Generic;
using System.Xml;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// The class which stores teh state of a Scrap Element configuration
    /// </summary>
    public class ScrapIteratorArgs : ParserIteratorArgs
    {
        /// <summary>
        /// The counter index if running in a loop
        /// </summary>
        public int NodeIndex { get; set; }

        /// <summary>
        /// The parsed and interpreted Scrap element from the configutaion file
        /// </summary>
        public ScrapElement ScrapConfigObj { get; set; }
        
        /// <summary>
        /// The raw Html node of HtmlDocument element
        /// </summary>
        public HtmlNodeNavigator WebHtmlNode { get; set; }

        /// <summary>
        /// The parent reference
        /// </summary>
        public ScrapIteratorArgs Parent { get; set; }

        /// <summary>
        /// Child reference
        /// </summary>
        public List<ScrapIteratorArgs> Child { get; set; }

        /// <summary>
        /// Child reference
        /// </summary>
        public List<ColumnScrapIteratorArgs> Columns { get; set; }

        /// <summary>
        /// Node index id
        /// </summary>
        public string NodeIndexId
        {
            get
            {
                ScrapIteratorArgs tmpIterator = this;
                List<string> idCreateList = new List<string>();

                while(tmpIterator != null)
                {
                    idCreateList.Add(tmpIterator.NodeIndex.ToString());
                    tmpIterator = tmpIterator.Parent;
                }

                return string.Join(".", idCreateList);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapIteratorArgs()
        {
            Child = new List<ScrapIteratorArgs>();
            Columns = new List<ColumnScrapIteratorArgs>();
            NodeIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void CloneData(ScrapIteratorArgs args)
        {
            this.NodeIndex = args.NodeIndex;
            this.ScrapConfigObj = args.ScrapConfigObj;
            this.WebHtmlNode = args.WebHtmlNode;
        }
    }
}
