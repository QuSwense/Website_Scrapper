using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Reader.Meta;

namespace DynamicDatabase.Config
{
    /// <summary>
    /// A Csv file which contains table metdata information.
    /// The Column information data contains metdata abpout a column
    /// </summary>
    public class ConfigDbColumn
    {
        [SplitIndex(2)]
        public string Display { get; set; }

        [SplitIndex(3)]
        public EConfigDbDataType DataType { get; set; }

        [SplitIndex(4)]
        public int Size { get; set; }

        [SplitIndex(5)]
        public int Precision { get; set; }

        [SplitIndex(6)]
        public bool Unique { get; set; }

        [SplitIndex(7)]
        public bool IsPrimaryKey { get; set; }
    }
}
