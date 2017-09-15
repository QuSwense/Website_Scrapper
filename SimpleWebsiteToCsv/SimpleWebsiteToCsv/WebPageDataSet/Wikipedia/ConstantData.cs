using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Wikipedia
{
    public static class ConstantData
    {
        public static CitationAttribute Citation { get; set; }

        static ConstantData()
        {
            Citation = new CitationAttribute();
            Citation.CitationString = "Wikipedia® is a registered trademark of the Wikimedia Foundation, Inc., a non-profit organization.";
            Citation.LogoOnlineUrl = "https://en.wikipedia.org/static/images/project-logos/enwiki.png";
        }
    }
}
