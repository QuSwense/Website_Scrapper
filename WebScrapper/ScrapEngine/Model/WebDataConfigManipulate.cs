using System.Collections.Generic;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The manipulate tags
    /// </summary>
    public class WebDataConfigManipulate
    {
        /// <summary>
        /// The tags which is used to split the data
        /// </summary>
        public List<WebDataConfigSplit> Splits { get; set; }

        /// <summary>
        /// The tag is used to trim the data
        /// </summary>
        public List<WebDataConfigTrim> Trims { get; set; }

        /// <summary>
        /// The tag is used to trim the data
        /// </summary>
        public List<WebDataConfigReplace> Replaces { get; set; }

        /// <summary>
        /// The tag is used to trim the data
        /// </summary>
        public List<WebDataConfigRegex> Regexes { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebDataConfigManipulate()
        {
            Splits = new List<WebDataConfigSplit>();
            Trims = new List<WebDataConfigTrim>();
            Replaces = new List<WebDataConfigReplace>();
            Regexes = new List<WebDataConfigRegex>();
        }
    }
}
