using QWWebScrap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWScrapEngine.Wikipedia.FIFA
{
    public class MembersAndListOfCodes : BaseScrapEngine
    {
        public MembersAndListOfCodes()
        {
            WebSegmentTree root = AddRoot("FIFAListOfCodes");
            root.AddReferencePath("//div[@class='mw-parser-output']/p[position() = 4]/text()[1]", 0);
            root.AddReferencePath("//div[@class='mw-references-wrap']//cite[position() = 1]", 0);
            root.AddUrl("https://en.wikipedia.org/wiki/List_of_FIFA_country_codes")
                .AddPath("//div[@id='mw-content-text']/div[@class='mw-parser-output']", 0);

            AddChildNodeTemplate(root, "MemberCodes", 4, 1, false);
            AddChildNodeTemplate(root, "NonMemberCodes", 5, 2);
            AddChildNodeTemplate(root, "IrregularCodes", 6, 3);
        }

        protected WebSegmentTree AddChildNodeTemplate(WebSegmentTree root, string name, int refIndx, int pathIndx, bool addConfederation = true)
        {
            WebSegmentTree child1 = root.AddChild(name);
            child1.AddReferencePath(string.Format("//div[@class='mw-parser-output']/p[position() = {0}]/text()[1]", refIndx), 0);
            child1.AddPath(string.Format(".//table[position() = {0}]//table[@class='wikitable']//tr[position() > 1]", pathIndx));

            child1.AddChild("Name").AddPath("td[position() = 1]/span/a", 0);
            child1.AddChild("NationalTeamUrl").AddPath("td[position() = 1]/span/a", 0, "href")
                .AddText("https://en.wikipedia.org", false, true);
            child1.AddChild("Code").AddPath("td[position() = 2]", 0);
            if (addConfederation) child1.AddChild("Confederation").AddPath("td[position() = 3]", 0);

            return child1;
        }
    }
}
