using ScrapEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    public class DbRowElement
    {
        #region References
        
        /// <summary>
        /// The list of column nodes
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ColumnNodeName)]
        public List<ColumnElement> Columns { get; set; }

        /// <summary>
        /// The parent node
        /// </summary>
        [DXmlParent]
        public ScrapElement Parent { get; set; }

        #endregion References
    }
}
