using ScrapEngine.Model;
using System.Collections.Generic;

namespace ScrapEngine.Interfaces
{
    public interface IScrapHtmlContext
    {
        /// <summary>
        /// Reference to the Scrapper Engine context class
        /// </summary>
        IScrapEngineContext EngineContext { get; }

        List<ScrapElement> RootScrapNodes { get; }

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
