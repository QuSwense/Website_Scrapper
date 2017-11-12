﻿using DynamicDatabase.Interfaces;
using DynamicDatabase.Model;
using ScrapEngine.Db;
using ScrapEngine.Model;
using System.Collections.Generic;

namespace ScrapEngine.Interfaces
{
    /// <summary>
    /// An interface to define application topic specific database context
    /// </summary>
    public interface IScrapDbContext
    {
        #region Properties

        /// <summary>
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        IScrapEngineContext ParentEngine { get; }

        /// <summary>
        /// Read the Database configuration
        /// </summary>
        DynamicAppDbConfig MetaDbConfig { get; }

        /// <summary>
        /// The configuration for web scrapping
        /// </summary>
        IDbContext WebScrapDb { get; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize(IScrapEngineContext parent);

        #endregion Constructor

        /// <summary>
        /// Add or update the data scrapped from the webpages including the metadata information
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="row"></param>
        void AddOrUpdate(string tableName, List<TableDataColumnModel> row, EWebDataConfigType scrapType);

        /// <summary>
        /// Load partial
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        void LoadPartial(WebDataConfigScrap webScrapConfigObj);
    }
}
