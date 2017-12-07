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
        [DXmlAttribute(ScrapXmlConsts.QueryAttributeName, IsMandatory = true)]
        public string Query { get; set; }

        /// <summary>
        /// It is a check that if the data is empty or null then remove it from to be used
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsEmptyOrNullAttributeName)]
        public bool IsEmptyOrNull { get; set; }

        /// <summary>
        /// Parent
        /// </summary>
        public DbchangeElement Parent { get; set; }
    }
}
