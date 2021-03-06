﻿using System;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// The class to iterate columns
    /// </summary>
    public class ColumnScrapStateModel : ParserStateModel
    {
        

        /// <summary>
        /// Pre process column config
        /// </summary>
        public virtual void PreProcess()
        {
            int level = 0;
            ScrapIteratorArgs required = Parent;
            while(required != null && level < ColumnElementConfig.Level)
            {
                ++level;
                required = required.Parent;
            }

            required.Columns[ColumnElementConfig.ColumnIndex].PreProcess();
        }

        public virtual string GetDataIterator() { return null;  }

        public static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
