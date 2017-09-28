using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QWCommonDST.Helper;

namespace QWWebScrap.Model
{
    /// <summary>
    /// This class is used to store the location of a webpage document.
    /// It contains ahint of the absolute Uri / path of the resource.
    /// It can be an online link or a offline file
    /// It can refer to any resource like png, img, html, csv, ...
    /// </summary>
    public class UriHint
    {
        /// <summary>
        /// This link refers to the network resource and it mostly handles "http" / "https" type
        /// </summary>
        public Uri Online { get; protected set; }

        /// <summary>
        /// This link refers to a local resource it mostly handles "file"
        /// When Opening a file using the offline link with classes like <see cref="System.IO.Stream"/> type use
        /// <see cref="Uri.AbsolutePath"/> as these classes cannot handles full Uri locator protocol
        /// </summary>
        public Uri Offline { get; protected set; }

        /// <summary>
        /// The type of resource that this class refers to
        /// </summary>
        public EUriResourceType ResourceType { get; protected set; }

        /// <summary>
        /// Default constructor required for any list operation.
        /// A parameterless constructor is different to Default constructor
        /// </summary>
        public UriHint() { }

        /// <summary>
        /// A constructor to initialize the class with url
        /// </summary>
        /// <param name="absoluteUrl"></param>
        /// <param name="isonline"></param>
        public UriHint(string absoluteUrl, EUriResourceType resType = EUriResourceType.HTML, bool isonline = true)
        {
            ResourceType = resType;
            if (isonline) SetOnline(absoluteUrl);
            else Offline = new Uri(absoluteUrl);
        }

        /// <summary>
        /// A seperate method to set and create online uri
        /// </summary>
        /// <param name="absoluteUrl"></param>
        protected void SetOnline(string absoluteUrl)
        {
            Online = new Uri(absoluteUrl);
            GenerateOffline();
        }

        /// <summary>
        /// This method automatically generates a offline url from Onlien Url if the flag 
        /// <see cref="QWSettings.DoGenerateOfflineUriFromOnline"/> is set.
        /// </summary>
        protected void GenerateOffline()
        {
            if (Online == null) throw new Exception("The Online Url is not set");

            if(QWSettings.DoGenerateOfflineUriFromOnline)
            {
                string url = Path.Combine(QWSettings.PathHelper.GetWebOfflineRootFolder(),
                    Online.DnsSafeHost.PathVerifyAndClean(), 
                    string.Format("{0}.{1}", Online.Segments.Last().PathVerifyAndClean(),
                    ResourceType.ToString().ToLower()));

                Offline = new Uri(url);
            }
        }
    }
}
