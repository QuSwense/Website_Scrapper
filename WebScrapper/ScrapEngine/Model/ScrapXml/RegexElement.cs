using ScrapEngine.Interfaces;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model
{
    public class RegexElement : ManipulateChildElement
    {
        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute("pattern", IsMandatory = true)]
        public string Pattern { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute("index")]
        public int Index { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegexElement()
        {
            Index = 0;
            manipulateType = EManipulateType.Regex;
        }
    }
}
