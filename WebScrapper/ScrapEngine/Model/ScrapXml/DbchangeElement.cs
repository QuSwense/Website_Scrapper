﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapEngine.Common;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// The Manipulate child node will manipulate data and intercation with database to 
    /// modify the data
    /// </summary>
    public class DbchangeElement : ManipulateChildElement
    {
        /// <summary>
        /// The Existence query clase
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IsExistsAttributeName)]
        public string IsExistsQuery { get; set; }

        /// <summary>
        /// The Exists node which checks the existence of node
        /// </summary>
        [DXmlElement(ScrapXmlConsts.DbchangeSelectNodeName)]
        public DbchangeSelectElement Select { get; set; }
    }
}
