using ScrapEngine.Common;
using ScrapEngine.Model.Scrap;
using System;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// A mnipulation Child element
    /// </summary>
    public class RegexElement : ConfigElementBase
    {
        #region Xml Attributes

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.PatternAttributeName, IsMandatory = true)]
        [DXmlNormalize]
        public string Pattern { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IndexAttributeName)]
        public string IndexString { get; set; }

        #endregion Xml Attributes

        #region Calculated

        /// <summary>
        /// The index of the regex matched groups. '*'(-1) for all of them 
        /// The index of the last elelment is represented by -2.
        /// </summary>
        public int Index
        {
            get
            {
                if (IndexString == ScrapXmlConsts.AllValue) return -1;
                else if (IndexString == ScrapXmlConsts.LastIndexValue) return -2;
                else if (string.IsNullOrEmpty(IndexString)) return 0;
                return Convert.ToInt32(IndexString);
            }
        }

        #endregion Calculated
    }
}
