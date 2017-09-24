namespace SimpleWebsiteScrapper.DataSet
{
    /// <summary>
    /// The class which stores the reference to the data source in a webpage.
    /// This class can completely identify a data node
    /// </summary>
    public class HtmlReferenceData
    {
        /// <summary>
        /// This is an absolute path
        /// </summary>
        public string AbsoluteUri { get; set; }

        /// <summary>
        /// The XPath link 
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// The attribute
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// The index of colelction
        /// </summary>
        public int CollectionIndex { get; set; }

        /// <summary>
        /// CReate anew instance of the class
        /// </summary>
        /// <returns></returns>
        public static HtmlReferenceData New() => new HtmlReferenceData();
    }
}
