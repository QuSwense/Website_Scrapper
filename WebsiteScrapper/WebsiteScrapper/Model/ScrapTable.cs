using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteScrapper.Model
{
    public class ScrapTable : ScrapObject<List<List<string>>>
    {
        public List<ScrapString> Headers { get; set; }
        public List<ScrapString> FirstRow { get; set; }
    }
}
