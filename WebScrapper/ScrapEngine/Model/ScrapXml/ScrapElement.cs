using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model
{
    public abstract class ScrapElement
    {
        /// <summary>
        /// The list of child Scrap element
        /// </summary>
        public List<ScrapElement> Scraps { get; set; }

        /// <summary>
        /// The name of the node
        /// </summary>
        [DXmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// The url
        /// </summary>
        [DXmlAttribute("url")]
        public string Url { get; set; }

        /// <summary>
        /// The type of the scrap node
        /// </summary>
        [DXmlAttribute("doupdateonly")]
        public bool DoUpdateOnly { get; set; }

        /// <summary>
        /// The list of column nodes
        /// </summary>
        public List<ColumnElement> Columns { get; set; }

        /// <summary>
        /// The parent node
        /// </summary>
        public ScrapElement Parent { get; set; }

        /// <summary>
        /// Get the level with respect to the parent Scrap node
        /// </summary>
        public int Level
        {
            get
            {
                int level = 0;
                ScrapElement tmpObj = this;
                while (tmpObj.Parent != null)
                {
                    tmpObj = tmpObj.Parent;
                    ++level;
                }

                return level;
            }
        }

        public ScrapElement()
        {
            Scraps = new List<ScrapElement>();
            Columns = new List<ColumnElement>();
            DoUpdateOnly = false;
        }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName
        {
            get
            {
                ScrapElement tmpObj = this;

                while (tmpObj != null)
                {
                    if (!string.IsNullOrEmpty(tmpObj.Name))
                    {
                        return tmpObj.Name;
                    }

                    tmpObj = tmpObj.Parent;
                }

                return string.Empty;
            }
        }
    }
}
