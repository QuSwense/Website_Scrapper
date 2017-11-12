using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrapEngine.Interfaces
{
    public interface IScrapHtmlContext
    {
        /// <summary>
        /// Reference to the Scrapper Engine context class
        /// </summary>
        IScrapEngineContext EngineContext { get; }

        /// <summary>
        /// The web data rules configuration
        /// </summary>
        WebDataConfig ScrapperRulesConfig { get; }

        /// <summary>
        /// Constructor initializes with parent engine
        /// </summary>
        /// <param name="engineContext"></param>
        void Initialize(IScrapEngineContext engineContext);

        /// <summary>
        /// Execute
        /// </summary>
        void Run();
    }
}
