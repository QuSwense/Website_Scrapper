using DynamicDatabase.Scrap;
using ScrapEngine.Bl;
using ScrapEngine.Db;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrapEngine.Interfaces
{
    public interface IScrapEngineContext
    {
        /// <summary>
        /// The folder path for ScrapperApps. If empty assume current
        /// </summary>
        string ScrapperFolderPath { get; }

        /// <summary>
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        string AppTopic { get; }

        /// <summary>
        /// Application config
        /// </summary>
        ApplicationConfig AppConfig { get; }

        /// <summary>
        /// A generic database config
        /// </summary>
        DynamicDbConfig GenericDbConfig { get; }

        /// <summary>
        /// The Database class layer
        /// </summary>
        ScrapDbContext WebDbContext { get; }

        /// <summary>
        /// Web scrapper html context
        /// </summary>
        WebScrapHtmlContext WebScrapHtml { get; }

        /// <summary>
        /// Initialize the engine context
        /// </summary>
        /// <param name="appTopic"></param>
        /// <param name="sqldb"></param>
        void Initialize(string folderPath, string appTopic, ApplicationConfig appGenericConfig, DynamicDbConfig genericDbConfig);
    }
}
