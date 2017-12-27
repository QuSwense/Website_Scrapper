using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    public class DbRowElement : ConfigElementBase
    {
        #region IConfigElement Implementation

        /// <summary>
        /// A set of child elements in order of occurance
        /// </summary>
        [DXmlElement(ScrapXmlConsts.ColumnNodeName)]
        [DXmlElement(ScrapXmlConsts.GroupColumnNodeName)]
        public override List<IConfigElement> Children { get; set; }

        #endregion IConfigElement Implementation
    }
}
