using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteScrapper.common
{
    public class WebScrapperException
    {
        public static void Assert(Func<bool> action, string message)
        {
            if (!action()) throw new Exception(message);
        }

        public static void IsNullEmpty(string obj, string message)
        {
            if (string.IsNullOrEmpty(obj)) throw new Exception(message);
        }

        public static void IsValidId(string obj, string message)
        {
            string validChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567";
            if (string.IsNullOrEmpty(obj)) throw new Exception(message);
            for(int indx = 0; indx < obj.Length; ++indx)
            {
                if(validChars.Contains(obj[indx])) throw new Exception(message);
            }
        }

        public static void IsValidEnum(string obj, Type enumType, string message)
        {
            if(!Enum.IsDefined(enumType, obj)) throw new Exception(message);
        }
    }
}
