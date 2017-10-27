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
        /// <summary>
        /// For Web Scrap Create Tables as well as the meta tables
        /// 1. Create the tables first as per the config file
        /// 2. Create a meta table for each table
        /// </summary>
        /// <param name="TableColumnConfigs"></param>
        public override void CreateTable(
            Dictionary<string, Dictionary<string, ConfigDbColumn>> TableColumnConfigs)
        {
            base.CreateTable(TableColumnConfigs);

            // Create meta tables
            foreach (var item in TableColumnConfigs)
            {
                CreateTable<DbMetaTableModel>(item.Key);
                CreateTable<DbMetaTableColumnsModel>(item.Key);
                CreateTable<DbMetaTableRowModel>(item.Key);
            }
        }

        /// <summary>
        /// Create a table using the type. The name of the table is as passed in the argument + the <see cref="DDTableAttribute"/>
        /// property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        public override void CreateTable<T>(string tableName = null)
        {
            Type tableType = typeof(T);
            DDTableAttribute tableattr = tableType.GetCustomAttribute<DDTableAttribute>();

            if(tableattr != null)
                tableName += tableattr.Name;

            CreateTable(tableName,
                (dynTable) => dynTable.CreateTable(
                    tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance)));
        }

        /// <summary>
        /// Add or update
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        public void AddOrUpdateRows(string name, List<TableDataColumnModel> row)
        {
            Type tableMetaType = typeof(DbMetaTableModel);
            DDTableAttribute tableAttr = tableMetaType.GetCustomAttribute<DDTableAttribute>();

            if (tableAttr == null) throw new Exception("Table name attribute on DbMetaTableModel class not found");
            if (!Tables.ContainsKey(tableAttr.Name)) LoadMetadata(tableAttr.Name);

            Tables[tableAttr.Name].AddorUpdate(row);

            Type tableRowMetaType = typeof(DbMetaTableRowModel);
            DDTableAttribute tableRowAttr = tableMetaType.GetCustomAttribute<DDTableAttribute>();

            if (tableRowAttr == null) throw new Exception("Table name attribute on DbMetaTableModel class not found");

            foreach (var item in row)
            {
                DbMetaTableRowModel tableRow = new DbMetaTableRowModel();
                tableRow.ColumnName = item.Name;
                tableRow.PrimaryKey = item.RowIndex;
                Tables[name + tableRowAttr.Name].AddorUpdate(row);
            }
        }
    }
}
