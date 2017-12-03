using System.Collections.Generic;

namespace WebCommon.Extn
{
    public static class ListClassExtension
    {
        /// <summary>
        /// Getwthe indexed 
        /// </summary>
        /// <param name="strObj"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T IndexedOrDefault<T>(this List<T> listObj, int index)
        {
            if (listObj == null || listObj.Count <= 0 || listObj.Count <= index) return default(T);

            return listObj[index];
        }

        /// <summary>
        /// Format the string from the list of data in string
        /// </summary>
        /// <param name="paramArgs"></param>
        /// <param name="format"></param>
        public static string Format(this List<string> paramArgs, string format)
        {
            return string.Format(format, paramArgs.ToArray());
        }
    }
}
