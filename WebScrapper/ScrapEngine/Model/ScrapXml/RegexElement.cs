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
        public string IndexString { get; set; }

        /// <summary>
        /// The index of the regex matched groups. '*'(-1) for all of them 
        /// </summary>
        public int Index
        {
            get
            {
                if (IndexString == "*") return -1;
                else if (string.IsNullOrEmpty(IndexString)) return 0;
                return Convert.ToInt32(IndexString);
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegexElement()
        {
            IndexString = "0";
            manipulateType = EManipulateType.Regex;
        }
    }
}
