using System;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// The class to iterate columns
    /// </summary>
    public class ColumnScrapIteratorArgs : ParserIteratorArgs
    {
        /// <summary>
        /// The unique id for each columns scrapped in a loop
        /// </summary>
        public string NodeIndexId { get; set; }

        public ColumnElement ColumnElementConfig { get; set; }

        public ManipulateHtmlData ResultColumnScrap { get; set; }

        /// <summary>
        /// The parent Scrap node
        /// </summary>
        public ScrapIteratorArgs Parent { get; set; }

        public virtual void PreProcess() { }
        public virtual string GetDataIterator() { return null;  }

        public static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
