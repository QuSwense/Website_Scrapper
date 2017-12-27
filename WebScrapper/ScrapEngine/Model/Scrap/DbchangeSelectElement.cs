using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using System.Diagnostics;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// The Select node which selects data from the database and replaces the previous data
    /// </summary>
    public class DbchangeSelectElement : ConfigElementBase
    {
        #region Xml Attributes

        /// <summary>
        /// The name of the table from which the data to be validated
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.QueryAttributeName, IsMandatory = true)]
        public string Query { get; set; }

        /// <summary>
        /// It is a check that if the data is empty or null then remove it from to be used
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsEmptyOrNullAttributeName,
            ConfigElementConsts.IsEmptyOrNullColumnDefault)]
        public bool IsEmptyOrNull { get; set; }

        #endregion Xml Attributes
    }
}
