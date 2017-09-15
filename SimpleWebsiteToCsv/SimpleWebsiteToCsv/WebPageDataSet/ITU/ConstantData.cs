using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.ITU
{
    public static class ConstantData
    {
        public static CitationAttribute Citation { get; set; }

        static ConstantData()
        {
            Citation = new CitationAttribute();
            Citation.Copyright = "© ITU 2017 All Rights Reserved";
            Citation.LogoOnlineUrl = "https://www.itu.int/PublishingImages/masterpage/logos/itu-logo.png";
        }
    }
}
