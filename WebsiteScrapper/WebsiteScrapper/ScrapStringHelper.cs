using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper
{
    public static class ScrapStringHelper
    {
        public static ScrapString Remove(this ScrapString obj, string text)
        {
            obj.Value = obj.Value.Replace(text, "").Trim();
            return obj;
        }
    }
}
