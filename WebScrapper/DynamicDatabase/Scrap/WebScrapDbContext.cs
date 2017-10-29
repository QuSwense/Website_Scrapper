using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Meta;
using DynamicDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        #region Create

        /// <summary>
        /// For Web Scrap Create Tables as well as the meta tables
        /// 1. Create the tables first as per the config file
        /// 2. Create a meta table for each table
        /// </summary>
        /// <param name="dynTableConfigs"></param>
        public override void CreateTable(
            Dictionary<string, Dictionary<string, ConfigDbColumn>> dynTableConfigs)
        {
            base.CreateTable(dynTableConfigs);

            // Create meta tables
            foreach (var item in dynTableConfigs)
            {
                // Create table metadata
                CreateTable<DbMetaTableModel>(item.Key);

                // Create metadata table row models
                CreateTable<DbMetaTableRowModel>(item.Key);

                // Create metadata table columns model
                CreateTable<DbMetaTableColumnsModel>(item.Key);

                // Add table column metadata
                AddOrUpdate(item.Key, item.Value);
            }
        }

        public 

        /// <summary>
        /// Create a table using the type. The name of the table is as passed in the argument + 
        /// the <see cref="DDTableAttribute"/> property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        public override void CreateTable<T>(string tableName = null)
        {
            Type tableType = typeof(T);
            DDTableAttribute tableattr = tableType.GetCustomAttribute<DDTableAttribute>();

            if (tableattr != null)
                tableName += tableattr.Name;

            CreateTable(tableName,
                (dynTable) => dynTable.CreateTable(
                    tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance)));
        }

        #endregion Create

        #region Load

        /// <summary>
        /// Use this method to load data
        /// Load the table data and metdata from the database
        /// </summary>
        /// <param name="name"></param>
        public override void Load(string name)
        {
            // Load the table with data
            base.Load(name);

            // Load web scrap metdata tables
            Load<DbMetaTableModel>(name);

            // Load metadata table row models
            Load<DbMetaTableRowModel>(name);

            // Load metadata table columns model
            Load<DbMetaTableColumnsModel>(name);
        }

        /// <summary>
        /// Load the data type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        public override void Load<T>(string name)
        {
            Type tableType = typeof(T);
            DDTableAttribute tableattr = tableType.GetCustomAttribute<DDTableAttribute>();

            if (tableattr != null)
                name += tableattr.Name;
            base.Load(name);
        }

        #endregion Load

        #region Insert

        /// <summary>
        /// Add or update
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        public override void AddOrUpdate(string name, List<TableDataColumnModel> row)
        {
            // Update the actual data
            base.AddOrUpdate(name, row);

            // Load web scrap metdata tables
            AddOrUpdate<DbMetaTableModel>(name, row);

            // Load metadata table row models
            AddOrUpdate<DbMetaTableRowModel>(name, row);

            // Load metadata table columns model
            AddOrUpdate<DbMetaTableColumnsModel>(name, row);
        }

        /// <summary>
        /// Add or update
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        public override void AddOrUpdate<T>(string name, List<TableDataColumnModel> row)
        {
            Type tableMetaType = typeof(T);
            DDTableAttribute tableAttr = tableMetaType.GetCustomAttribute<DDTableAttribute>();
            
            if (tableAttr != null)
                name += tableAttr.Name;

            // Update the actual data
            base.AddOrUpdate(name, row);
        }

        /// <summary>
        /// Add or update columns related information
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        private void AddOrUpdate(string name, Dictionary<string, ConfigDbColumn> colMetaItems)
        {
            if(Tables.ContainsKey(name))
            {
                foreach (var item in colMetaItems)
                {
                    List<string> pks = new List<string>(new string[] {
                        name, item.Key
                    });
                    List<string> row = new List<string>(new string[] {
                        name, item.Key, item.Value.Display
                    });

                    Tables[name].AddorUpdate(pks, row);
                }
            }
        }

        #endregion Insert
    }
}
