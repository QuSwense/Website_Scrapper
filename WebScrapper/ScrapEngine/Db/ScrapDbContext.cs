using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebScrapper.Db.Ctx;

namespace ScrapEngine.Db
{
    public class ScrapDbContext
    {
        public DynamicDbConfig MetaConfig { get; set; }
        //public SqliteDbContext

        public ScrapDbContext() { }

        public void CreateMetadataTables()
        {

        }
    }
}
