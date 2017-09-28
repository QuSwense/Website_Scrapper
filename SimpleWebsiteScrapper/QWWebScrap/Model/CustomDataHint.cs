using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.Model
{
    /// <summary>
    /// Apart from scrapping the data from the websites, sometimes it is important to provide custom data
    /// </summary>
    public class CustomDataHint
    {
        /// <summary>
        /// The actual data string
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Flag to set if the custom text should be appended to the Html Hint text or
        /// prepended. Default is false
        /// </summary>
        public bool DoAppendToHtmlHint { get; set; }

        public bool IsPath { get; set; }

        /// <summary>
        /// Default constructor must for IList. Different to Parameterless constructor
        /// </summary>
        public CustomDataHint() { }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="text"></param>
        /// <param name="append"></param>
        public CustomDataHint(string text, bool append = true, bool ispath = false)
        {
            Text = text;
            DoAppendToHtmlHint = append;
            IsPath = ispath;
        }
    }
}
