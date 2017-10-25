using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Db.Ctx
{
    public class DbDoubleDataType : DbDataType
    {
        public int Count { get; set; }
        public int CountAfterDecimal { get; set; }

        public DbDoubleDataType() { }

        public DbDoubleDataType(int count, int afterDecimal)
        {
            Count = count;
            CountAfterDecimal = afterDecimal;
        }
    }
}
