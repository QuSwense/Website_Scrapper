using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Db.Ctx
{
    public class DbIntDataType : DbDataType
    {
        public int Count { get; set; }

        public DbIntDataType() { }

        public DbIntDataType(int count)
        {
            this.Count = count;
        }
    }
}
