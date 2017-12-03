using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCommon.Const
{
    /// <summary>
    /// ASCII character constants both escaped and unescaped
    /// </summary>
    public class ASCIICharacters
    {
        public static string TabString { get { return "\\t"; } }
        public static string Tab { get { return "\t"; } }
        public static string NewLineString { get { return "\\n"; } }
        public static string NewLine { get { return "\n"; } }
    }
}
