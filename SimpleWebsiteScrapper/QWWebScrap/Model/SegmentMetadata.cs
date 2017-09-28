using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QWWebScrap.Model
{
    /// <summary>
    /// This class defines the metadata of the scrapped data from webpages.
    /// It also mentions if the metadata is for the children as well unless the flag is false.
    /// </summary>
    public class SegmentMetadata
    {
        /// <summary>
        /// A flag to specify if the metadata is for only current segment or for
        /// all Children recursivley.
        /// Default is false.
        /// </summary>
        public bool OnlyForCurrentSegment { get; set; }

        /// <summary>
        /// Refers to the location of the resource
        /// </summary>
        public HtmlPathHint HtmlPath { get; set; }

        /// <summary>
        /// Custom data provider
        /// </summary>
        public CustomDataHint Custom { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SegmentMetadata() { OnlyForCurrentSegment = false; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <returns></returns>
        public static SegmentMetadata New() => new SegmentMetadata();

        /// <summary>
        /// A helper function add new Uri locator hint
        /// </summary>
        /// <param name="url"></param>
        /// <param name="resType"></param>
        /// <param name="isonline"></param>
        public SegmentMetadata AddUrl(string url, EUriResourceType resType = EUriResourceType.HTML, bool isonline = true)
        {
            if (HtmlPath == null) HtmlPath = new HtmlPathHint();
            HtmlPath.Url = new UriHint(url, resType, isonline);

            return this;
        }

        /// <summary>
        /// Add path for the locator
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="collIndex"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public SegmentMetadata AddPath(string xpath, int collIndex = -1, string attribute = null)
        {
            if (HtmlPath == null) HtmlPath = new HtmlPathHint();
            HtmlPath.Path = new XPathHint(xpath, collIndex, attribute);

            return this;
        }

        public SegmentMetadata AddText(string text, bool isappend = true, bool ispath = false)
        {
            if (Custom == null) Custom = new CustomDataHint();
            Custom.DoAppendToHtmlHint = isappend;
            Custom.Text = text;
            Custom.IsPath = ispath;

            return this;
        }
    }
}
