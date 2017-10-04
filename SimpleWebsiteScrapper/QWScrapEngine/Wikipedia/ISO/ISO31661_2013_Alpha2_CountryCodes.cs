using QWWebScrap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWScrapEngine.Wikipedia.ISO
{
    public class ISO31661_2013_Alpha2_CountryCodes : BaseScrapEngine
    {
        public ISO31661_2013_Alpha2_CountryCodes()
        {
            WebSegmentTree root = AddRoot("ISO3166_1_Alpha_2");
            root.AddUrl("https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2");

            WebSegmentTree child = root;
            child.AddPath("//div[@id='mw-content-text']/div[1]/table[2]/tr[position() > 1]");

            child.AddChild("Code").AddPath("td[1]", 0, "id")
                .AddReferencePath("//div[@id='mw-content-text']/div[1]/ul[1]/li[1]", 0);
            child.AddChild("CountryName").AddPath("td[2]/a", 0, "title")
                .AddReferencePath("//div[@id='mw-content-text']/div[1]/ul[1]/li[2]", 0);
            child.AddChild("CountryWikiUrl").AddPath("td[2]/a", 0, "href")
                .AddText("https://en.wikipedia.org", false, true);
            child.AddChild("Year").AddPath("td[3]", 0)
                .AddReferencePath("//div[@id='mw-content-text']/div[1]/ul[1]/li[3]", 0);
            child.AddChild("ccTLD").AddPath("td[4]/a", 0, "title")
                .AddReferencePath("//div[@id='mw-content-text']/div[1]/ul[1]/li[4]", 0);
            child.AddChild("ccTLDWikiUrl").AddPath("td[4]/a", 0, "href")
                .AddText("https://en.wikipedia.org", false, true);
        }
    }
}
