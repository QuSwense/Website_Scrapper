using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Model.Parser
{
    public class ColumnScrapIteratorArgs
    {
        public int NodeIndex { get; set; }
        public ScrapElement ScrapConfig { get; set; }
        public XmlNode ScrapNode { get; set; }

        public virtual void PreProcess() { }
        public virtual string GetDataIterator(ColumnElement columnConfig) { return null;  }

        public static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
