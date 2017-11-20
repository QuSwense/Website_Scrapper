using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Model.Parser
{
    public class ColumnScrapIteratorFileArgs : ColumnScrapIteratorArgs
    {
        public string FileLine { get; set; }

        private string[] splitData;

        public override void PreProcess()
        {
            WebDataConfigScrapCsv ScrapConfigCsv = ScrapConfig as WebDataConfigScrapCsv;
            if (ScrapConfigCsv == null)
            {
                splitData = FileLine.Split(new char[] { ScrapConfigCsv.Delimiter[0] });
            }
        }

        public override string GetDataIterator(ColumnElement columnConfig)
        {
            return splitData[columnConfig.Index];
        }
    }
}
