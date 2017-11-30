﻿using ScrapEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    public class RegexReplaceElement : ManipulateChildElement
    {
        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.PatternAttributeName, IsMandatory = true)]
        public string Pattern { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.ReplaceNodeName)]
        public string Replace { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RegexReplaceElement()
        {
            Replace = string.Empty;
        }
    }
}
