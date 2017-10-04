using QWWebScrap.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWScrapEngine.ISO.ISO3166
{
    public class ISO3166_1IndividualCountryCode : BaseScrapEngine
    {
        public ISO3166_1IndividualCountryCode()
        {
            WebSegmentTree root = AddRoot("ISO3166_1:2013_CountryInfo");

            using (TextReader txtReader = new StreamReader(@"Wikipedia\ISO\ISO3166_1_Alpha_2"))
            {
                string line = null;
                int row = 0;

                while ((line = txtReader.ReadLine()) != null)
                {
                    if (row == 0) continue;

                    string[] split = line.Split(new char[] { ',' });
                    WebSegmentTree child = root.AddChild(split[1]);
                    child.AddUrl(string.Format("https://www.iso.org/obp/ui/#iso:code:3166:{0}", split[0]));



                    row++;
                }
            }
        }
    }
}
