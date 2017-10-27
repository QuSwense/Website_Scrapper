using DynamicDatabase.Scrap;
using ScrapEngine.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrapEngine.Interfaces
{
    public interface IScrapEngineContext
    {
        string AppTopic { get; }
        DynamicDbConfig MetaDbConfig { get; }
        WebScrapDbContext WebScrapDb { get; }
        void Initialize(string appTopic, string sqldb);
    }
}
