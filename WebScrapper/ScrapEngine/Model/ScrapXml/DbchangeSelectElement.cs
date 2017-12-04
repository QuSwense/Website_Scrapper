using ScrapEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// The Select node which selects data from the database and replaces the previous data
    /// </summary>
    public class DbchangeSelectElement
    {
        /// <summary>
        /// The name of the table from which the data to be validated
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.TableAttributeName)]
        public string Table { get; set; }

        /// <summary>
        /// Column name in the table to validate from 
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.ColAttributeName)]
        public string Column { get; set; }

        /// <summary>
        /// Column name in the table to validate from 
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.InnerJoinCriteriaAttributeName)]
        public string InnerJoinCriteria { get; set; }

        /// <summary>
        /// Parent
        /// </summary>
        public DbchangeExistsElement Parent { get; set; }
    }
}
