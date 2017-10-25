using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Db.Config;

namespace WebScrapper.Db
{
    public class DbFactory
    {
        public static DbGeneratorBL GetGenerator(string dbType, DbConfig configModel)
        {
            switch(dbType)
            {
                case "sqlite":
                    return new SqlliteDbGeneratorBL(configModel);
                default:
                    return null;
            }
        }
    }
}
