using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Meta;
using DynamicDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCommon.Extn;

namespace DynamicDatabase.Scrap
{
    /// <summary>
    /// The database context class which helps in to process and manipulate web
    /// scrap database
    /// </summary>
    public class WebScrapDbContext : DynamicDbContext
    {
        /// <summary>
        /// For Web Scrap Create Tables as well as the meta tables
        /// </summary>
        /// <param name="TableColumnConfigs"></param>
        public override void CreateTable(
            Dictionary<string, Dictionary<string, ConfigDbColumn>> TableColumnConfigs)
        {
            base.CreateTable(TableColumnConfigs);

            // Create meta tables
            foreach (var item in TableColumnConfigs)
            {
                CreateTable(item.Key + "mdt", item.Value);
            }
        }
    }
}
