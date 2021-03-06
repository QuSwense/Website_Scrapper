﻿using ScrapEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// The Exists database change node
    /// </summary>
    public class DbchangeExistsElement
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
        /// Select database change node
        /// </summary>
        [DXmlElement(ScrapXmlConsts.DbchangeSelectNodeName)]
        public DbchangeSelectElement Select { get; set; }
    }
}
