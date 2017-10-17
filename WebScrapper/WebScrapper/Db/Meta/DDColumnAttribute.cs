using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper.Db.Meta
{
    public class DDColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public DDColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
