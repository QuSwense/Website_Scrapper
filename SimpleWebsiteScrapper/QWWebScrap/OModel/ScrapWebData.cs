using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.OModel
{
    public class ScrapWebData
    {
        public string id { get; set; }
        public List<ScrapMetadata> Copyrights { get; set; }
        public List<ScrapMetadata> References { get; set; }

        public List<List<ScrapWebData>> Nodes { get; set; }
        public ScrapMetadata Text { get; set; }
    }
}
