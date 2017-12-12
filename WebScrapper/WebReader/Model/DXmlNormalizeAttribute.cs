using System;
using System.Web;
using WebCommon.Const;

namespace WebReader.Model
{
    public class DXmlNormalizeAttribute : Attribute
    {
        public static string Normalize(string htmlValue)
        {
            if (string.IsNullOrEmpty(htmlValue)) return htmlValue;
            htmlValue = HttpUtility.HtmlDecode(htmlValue);

            return htmlValue.Replace(ASCIICharacters.NewLineString, ASCIICharacters.NewLine)
                .Replace(ASCIICharacters.TabString, ASCIICharacters.Tab);
        }
    }
}
