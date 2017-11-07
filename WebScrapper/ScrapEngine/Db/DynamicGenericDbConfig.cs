using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System.Collections;
using WebCommon.Error;
using WebCommon.PathHelp;
using WebReader.Csv;
using System.Linq;

namespace ScrapEngine.Db
{
    /// <summary>
    /// A generic database config file reader and store class
    /// </summary>
    public class DynamicGenericDbConfig
    {
        /// <summary>
        /// This data set stores the table informations
        /// </summary>
        public DbTablesDefinitionModel TableMetadataConfigs { get; protected set; }

        /// <summary>
        /// This data set stores the table column informations
        /// </summary>
        public DbTablesDefinitionModel TableColumnMetadataConfigs { get; protected set; }

        /// <summary>
        /// This data set stores the table column rows informations
        /// </summary>
        public DbTablesDefinitionModel TableColumnRowMetadataConfigs { get; protected set; }

        /// <summary>
        /// Get the name of the metdata table
        /// </summary>
        public string MetadataTableName
        {
            get
            {
                return TableMetadataConfigs.Keys.First();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DynamicGenericDbConfig()
        {
            TableMetadataConfigs = new DbTablesDefinitionModel("Table Metadata");
            TableColumnMetadataConfigs = new DbTablesDefinitionModel("Table column metadata");
            TableColumnRowMetadataConfigs = new DbTablesDefinitionModel("Table rows metadata");
        }

        /// <summary>
        /// Read the config files
        /// </summary>
        public void Read()
        {
            AppGenericConfigPathHelper.I.DbScriptsTableMdt.AssertExists();
            AppGenericConfigPathHelper.I.DbScriptsTableColumnMdt.AssertExists();
            AppGenericConfigPathHelper.I.DbScriptsTableColumnRowsMdt.AssertExists();

            TableMetadataConfigs = CSVReader.Read<DbTablesDefinitionModel>(AppGenericConfigPathHelper.I.DbScriptsTableMdt.FullPath);
            TableColumnMetadataConfigs = CSVReader.Read<DbTablesDefinitionModel>(AppGenericConfigPathHelper.I.DbScriptsTableColumnMdt.FullPath);
            TableColumnRowMetadataConfigs = CSVReader.Read<DbTablesDefinitionModel>(AppGenericConfigPathHelper.I.DbScriptsTableColumnRowsMdt.FullPath);

            AssertConfig(TableMetadataConfigs);
            AssertConfig(TableColumnMetadataConfigs);
            AssertConfig(TableColumnRowMetadataConfigs);
        }

        /// <summary>
        /// Validates and checks the table metdata object read from the file
        /// </summary>
        public void AssertConfig(object configObj)
        {
            IDictionary configDictionary = (IDictionary)configObj;
            IIdentity configIdentity = (IIdentity)configObj;
            if (configDictionary == null)
                throw new GenericDbConfigException(configIdentity.Name, GenericDbConfigException.EErrorType.NULL_STORE);

            if(configDictionary.Count <= 0)
                throw new GenericDbConfigException(configIdentity.Name, GenericDbConfigException.EErrorType.EMPTY_STORE);

            if (configDictionary.Keys.Count != 1)
                throw new GenericDbConfigException(configIdentity.Name, GenericDbConfigException.EErrorType.MULTIPLE_TABLE_STORE);
        }
    }
}
