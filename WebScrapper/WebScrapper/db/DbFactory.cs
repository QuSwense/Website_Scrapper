using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper.Db
{
    public class DbFactory
    {
        public static DbGeneratorBL GetGenerator(string dbType, DbConfigModel configModel)
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
