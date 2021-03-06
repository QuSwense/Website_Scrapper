﻿using ScrapEngine.Common;
using ScrapEngine.Model.Scrap;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    public class ReplaceElement : ConfigElementBase
    {
        #region Xml Attributes

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.InAttributeName, IsMandatory = true)]
        [DXmlNormalize]
        public string InString { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.OutAttributeName, IsMandatory = true)]
        [DXmlNormalize]
        public string OutString { get; set; }

        #endregion Xml Attributes
    }
}
