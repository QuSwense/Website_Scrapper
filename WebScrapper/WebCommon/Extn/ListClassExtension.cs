using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
