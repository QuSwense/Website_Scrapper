using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// This class is used to store the location of a webpage document.
    /// </summary>
    public class UrlProcessorNode : ProcessorNode<UrlProcessorNode>
    {
        /// <summary>
        /// The <see cref="Uri"/> property to refers to Online resource (network),
        /// usually follows http:// or https:// protocol
        /// </summary>
        public Uri Online { get; set; }

        /// <summary>
        /// The <see cref="Uri"/> property to refers to offline resource (a local path,
        /// usually follows file:// protocol)
        /// </summary>
        public Uri Offline { get; set; }

        /// <summary>
        /// A memebr method to clone data to 'this' node from another node
        /// </summary>
        /// <param name="node">The node from which to create the copy</param>
        /// <returns></returns>
        public override UrlProcessorNode Clone(UrlProcessorNode node)
        {
            if (node != null)
            {
                if (node.Online != null)
                    Online = new Uri(node.Online.AbsoluteUri);

                if(node.Offline != null)
                    Offline = new Uri(node.Offline.AbsoluteUri);
            }

            return this;
        }

        /// <summary>
        /// A utility method to create offline url string from the online Uri
        /// </summary>
        /// <param name="urlOnline"></param>
        /// <returns></returns>
        public string CreateOfflineUri()
            => Path.Combine("WebDataScrapped", Online.Segments.FirstOrDefault(),
                SanitizeStringForPath(Online.Segments.LastOrDefault()) + ".html");

        /// <summary>
        /// Return the Offline Absolute Uri or empty if not present
        /// </summary>
        /// <returns></returns>
        public string OfflineOrDefault() => (Offline == null) ? string.Empty : Offline.AbsoluteUri;

        /// <summary>
        /// Return the Online Absolute Uri or empty if not present
        /// </summary>
        /// <returns></returns>
        public string OnlineOrDefault() => (Online == null) ? string.Empty : Online.AbsoluteUri;

        /// <summary>
        /// Return the Offline Uri if not null, else return Online Uri 
        /// </summary>
        /// <returns></returns>
        public string OfflineOrOnlineOrDefault() => 
            (Offline != null) ? Offline.AbsoluteUri : (Online != null) ? Online.AbsoluteUri : string.Empty;

        /// <summary>
        /// Extract the Company name from the class name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetWebCompanyNameFromClassName(string name)
            => Regex.Split(name, @"(?<!^)(?=[A-Z])")[0];

        /// <summary>
        /// Normalize string fit for Path
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string SanitizeStringForPath(string value)
            => Regex.Replace(value, @"[;\&%\$\@]", "");
    }
}
