using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Db.Ctx
{
    public interface IDbContext
    {
        string GetDataType(Type propertyType);
    }
}
