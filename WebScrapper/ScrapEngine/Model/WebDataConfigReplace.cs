using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebReader.Model;

namespace ScrapEngine.Model
{
    public class WebDataConfigReplace
    {
        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute("in", IsMandatory = true)]
        public string InString { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute("out", IsMandatory = true)]
        public string OutString { get; set; }
    }
}
