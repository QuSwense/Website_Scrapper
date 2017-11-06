using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        public WebDataConfigSplit[] Splits { get; set; }

        /// <summary>
        /// The tag is used to trim the data
        /// </summary>
        public WebDataConfigTrim Trim { get; set; }
    }
}
