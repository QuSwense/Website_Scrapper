using ScrapEngine.Common;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// Validate the scrapped data against database. If not validated throw error.
    /// </summary>
    public class ValidateElement : ConfigElementBase
    {
        #region Xml Attributes

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

        #endregion Xml Attributes
    }
}
