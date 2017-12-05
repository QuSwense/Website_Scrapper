using ScrapEngine.Common;
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
        [DXmlAttribute(ScrapXmlConsts.DataAttributeName, IsMandatory = true)]
        public string Data { get; set; }

        /// <summary>
        /// This flag is related with the <see cref="Data"/> property which if true Split the string
        /// using the whole Data string, if false, split using the Character array
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.SplitAsStringAttributeName)]
        public bool SplitAsString { get; set; }

        /// <summary>
        /// The data node
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.IndexAttributeName)]
        public string IndexString { get; set; }

        /// <summary>
        /// The index of the split groups. '*'(-1) for all of them.
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

        /// <summary>
        /// Default constructor
        /// </summary>
        public SplitElement()
        {
            IndexString = "0";
            SplitAsString = false;
        }
    }
}
