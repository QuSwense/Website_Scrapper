using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.Model
{
    /// <summary>
    /// The main data class which represents a section of data that is to be
    /// scrapped from the webpage. It contains the information to scrap the data.
    /// It also contains metadata information like References, and Copyrights.
    /// </summary>
    public class WebSegmentTree
    {
        public static event Action<WebSegmentVisitorEventArgs> HandleVisitorData;

        public WebSegmentTree Parent { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// The list of copyrights informations. It is mandatory to refer to Copyright information,
        /// if you are scrapping and using the data elsewhere. One can either set it manually or can
        /// refer to the resource path
        /// </summary>
        public List<SegmentMetadataTree> Copyrights { get; set; }

        /// <summary>
        /// The list of copyrights informations. It makes it reliable to refer to References information,
        /// if you are scrapping and using the data elsewhere. One can either set it manually or can
        /// refer to the resource path
        /// </summary>
        public List<SegmentMetadataTree> References { get; set; }

        /// <summary>
        /// The Children
        /// </summary>
        public List<WebSegmentTree> Nodes { get; set; }

        public static void ResetEvent()
        {
            HandleVisitorData = null;
        }

        /// <summary>
        /// Refers to the resource path to extract the actual data
        /// </summary>
        public SegmentMetadata ActualData { get; set; }
        
        /// <summary>
        /// Add a new refrence metadata
        /// </summary>
        public SegmentMetadataTree AddReference()
        {
            if (References == null) References = new List<SegmentMetadataTree>();
            SegmentMetadataTree reference = new SegmentMetadataTree();
            reference.ParentWebTree = this;
            References.Add(reference);
            return reference;
        }

        /// <summary>
        /// Add a new refrence metadata
        /// </summary>
        public SegmentMetadataTree AddCopyright()
        {
            if (Copyrights == null) Copyrights = new List<SegmentMetadataTree>();
            SegmentMetadataTree copyright = new SegmentMetadataTree();
            copyright.ParentWebTree = this;
            References.Add(copyright);
            return copyright;
        }

        /// <summary>
        /// Add xpath data to reference
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="collIndex"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public SegmentMetadataTree AddReferencePath(string xpath, int collIndex = -1, string attribute = null)
        {
            if (References == null) References = new List<SegmentMetadataTree>();
            SegmentMetadataTree reference = AddReference();
            reference.AddData().AddPath(xpath, collIndex, attribute);
            return reference;
        }

        /// <summary>
        /// Add xpath data to reference
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="collIndex"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public SegmentMetadataTree AddReferenceUrl(string url, EUriResourceType resType = EUriResourceType.HTML, bool isonline = true)
        {
            if (References == null) References = new List<SegmentMetadataTree>();
            SegmentMetadataTree reference = AddReference();
            reference.AddData().AddUrl(url, resType, isonline);
            return reference;
        }

        /// <summary>
        /// Add xpath data to reference
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="collIndex"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public SegmentMetadataTree AddReference(string url, string xPath, int collIndex = -1, string attribute = null,
            EUriResourceType resType = EUriResourceType.HTML, bool isonline = true)
        {
            if (References == null) References = new List<SegmentMetadataTree>();
            SegmentMetadataTree reference = AddReference();
            reference.AddData().AddUrl(url, resType, isonline);
            reference.AddData().AddPath(xPath, collIndex, attribute);
            return reference;
        }

        /// <summary>
        /// Add xpath data to reference
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="collIndex"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public SegmentMetadataTree AddCopyrightPath(string xpath, int collIndex = -1, string attribute = null)
        {
            if (Copyrights == null) Copyrights = new List<SegmentMetadataTree>();
            SegmentMetadataTree copyright = AddCopyright();
            copyright.AddData().AddPath(xpath, collIndex, attribute);
            return copyright;
        }

        /// <summary>
        /// Add xpath data to reference
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="collIndex"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public SegmentMetadataTree AddCopyrightUrl(string url, EUriResourceType resType = EUriResourceType.HTML, bool isonline = true)
        {
            if (Copyrights == null) Copyrights = new List<SegmentMetadataTree>();
            SegmentMetadataTree copyright = AddCopyright();
            copyright.AddData().AddUrl(url, resType, isonline);
            return copyright;
        }

        public WebSegmentTree AddPath(string xpath, int collIndex = -1, string attribute = null)
        {
            if (ActualData == null) ActualData = new SegmentMetadata();
            ActualData.AddPath(xpath, collIndex, attribute);

            return this;
        }

        public WebSegmentTree AddUrl(string url, EUriResourceType resType = EUriResourceType.HTML, bool isonline = true)
        {
            if (ActualData == null) ActualData = new SegmentMetadata();
            ActualData.AddUrl(url, resType, isonline);

            return this;
        }

        public WebSegmentTree AddText(string text, bool isappend = true, bool ispath = false)
        {
            if (ActualData == null) ActualData = new SegmentMetadata();
            ActualData.AddText(text, isappend, ispath);

            return this;
        }

        /// <summary>
        /// Add a new child tree
        /// </summary>
        /// <returns></returns>
        public WebSegmentTree AddChild(string name)
        {
            if (Nodes == null) Nodes = new List<WebSegmentTree>();
            WebSegmentTree childNode = new WebSegmentTree();
            childNode.Id = name;
            childNode.Parent = this;
            Nodes.Add(childNode);

            return childNode;
        }
    }
}
