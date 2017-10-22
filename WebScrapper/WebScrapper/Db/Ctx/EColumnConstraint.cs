using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Db.Ctx
{
    public enum EColumnConstraint
    {
        NONE = 1,
        NOTNULL = 2,
        UNQIUE = 4,
        PRIMARYKEY = 8,
        DEFAULT = 16
    }
}
