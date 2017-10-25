using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper.Db.Meta
{
    public class DDTableAttribute : Attribute
    {
        public string Name { get; set; }

        public DDTableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
