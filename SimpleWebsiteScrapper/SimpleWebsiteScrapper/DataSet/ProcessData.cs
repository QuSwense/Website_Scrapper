namespace SimpleWebsiteScrapper.DataSet
{
    /// <summary>
    /// The class whcih contains the main data
    /// </summary>
    public class ProcessData
    {
        /// <summary>
        /// The reference to the Html link data reference which can completely identify the 
        /// resource
        /// </summary>
        public HtmlReferenceData Link { get; set; }

        /// <summary>
        /// The textual data scrapped from the website
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcessData()
        {
            Link = new HtmlReferenceData();
        }
    }
}
