using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.OModel
{
    public class ScrapMetadata
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public List<List<ScrapMetadata>> Nodes { get; set; }
    }
}
