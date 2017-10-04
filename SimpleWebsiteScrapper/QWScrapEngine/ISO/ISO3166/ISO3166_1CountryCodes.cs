using QWWebScrap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWScrapEngine.ISO.ISO3166
{
    public class ISO3166_1CountryCodes : BaseScrapEngine
    {
        public ISO3166_1CountryCodes()
        {
            WebSegmentTree root = AddRoot("ISO3166_1:2013");
            SegmentMetadataTree refTree = root.AddReferenceUrl("https://www.iso.org/iso-3166-country-codes.html");
            refTree.AddChildPath("//body/div[1]/div[2]//div[2]/p[1]", 0);
            refTree.AddChildPath("//body/div[1]/div[2]//div[2]/p[2]", 0);
            refTree.AddChildPath("//body/div[1]/div[3]//div[2]/p[3]", 0);
            refTree.AddChildPath("//body/div[1]/div[3]//div[2]/p[4]", 0);
        }
    }
}
