using ScrapEngine.Bl.Parser;
using ScrapEngine.Model.Scrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Factory
{
    /// <summary>
    /// A factory class to map the config element with the parser class which processes the
    /// config element
    /// </summary>
    public class ConfigParserFactory
    {
        #region Properties

        /// <summary>
        /// A factory property which maps the config element to the parser logic class
        /// </summary>
        private Dictionary<string, AppTopicConfigParser> factory;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigParserFactory()
        {
            factory = new Dictionary<string, AppTopicConfigParser>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Construct the parser factory mapping
        /// </summary>
        public void ConstructParserFactory()
        {
            factory.Add(typeof(ColumnElement).Name, new ScrapColumnConfigParser());
            factory.Add(typeof(DbchangeElement).Name, new ScrapDbChangeConfigParser());
            factory.Add(typeof(DbchangeSelectElement).Name, new ScrapDbChangeSelectConfigParser());
            factory.Add(typeof(DbRowElement).Name, new DbRowConfigParser());
            factory.Add(typeof(GroupColumnElement).Name, new GroupColumnConfigParser());
            factory.Add(typeof(HtmlDecodeElement).Name, new ScrapHtmlDecodeConfigParser());
            factory.Add(typeof(ManipulateElement).Name, new ScrapManipulateChildConfigParser());
            factory.Add(typeof(PurgeElement).Name, new ScrapPurgeConfigParser());
            factory.Add(typeof(RegexElement).Name, new ScrapRegexConfigParser());
            factory.Add(typeof(RegexReplaceElement).Name, new ScrapRegexReplaceParser());
            factory.Add(typeof(ReplaceElement).Name, new ScrapReplaceConfigParser());
            factory.Add(typeof(ScrapCsvElement).Name, new ScrapCsvConfigParser());
            factory.Add(typeof(ScrapHtmlTableElement).Name, new ScrapHtmlTableConfigParser());
            factory.Add(typeof(SplitElement).Name, new ScrapSplitConfigParser());
            factory.Add(typeof(TrimElement).Name, new ScrapTrimConfigParser());
            factory.Add(typeof(ValidateElement).Name, new ScrapValidateConfigParser());
        }

        /// <summary>
        /// The main parser method call for parsing and interpreting the config element
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public AppTopicConfigParser Parser(object obj)
        {
            return factory[obj.GetType().Name];
        }

        #endregion Methods
    }
}
