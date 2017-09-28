using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWCommonDST.Cache
{
    public class HtmlDocCache
    {
        public HtmlDocument Document { get; set; }
        public bool IsOfflineUrl { get; set; }
        public Uri UrlUsed { get; set; }
    }
}
