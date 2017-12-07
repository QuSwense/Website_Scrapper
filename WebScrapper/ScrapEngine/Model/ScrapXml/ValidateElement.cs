using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// Validate the scrapped data
    /// </summary>
    public class ValidateElement
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

        #region References

        /// <summary>
        /// Parent column element
        /// </summary>
        public ColumnElement Parent { get; set; }

        #endregion References
    }
}
