using ScrapEngine.Interfaces;
using ScrapEngine.Model.ScrapXml;
using System;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The class for manipulating a scrapped data
    /// The element is used to manipulate the data by splitting the scrapped data
    /// and picking a split element by index.
    /// </summary>
    public class SplitElement : ManipulateChildElement
    {
        /// <summary>
        /// The split string by which the scrapped data needs to be splitted
        /// </summary>
        [DXmlAttribute("data", IsMandatory = true)]
        public string Data { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute("index")]
        public string IndexString { get; set; }

        /// <summary>
        /// The index of the split groups. '*'(-1) for all of them.
        /// The index of the last elelment is represented by -2.
        /// </summary>
        public int Index
        {
            get
            {
                if (IndexString == "*") return -1;
                else if (IndexString == "last") return -2;
                else if (string.IsNullOrEmpty(IndexString)) return 0;
                return Convert.ToInt32(IndexString);
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SplitElement()
        {
            IndexString = "0";
            manipulateType = EManipulateType.Split;
        }
    }
}
