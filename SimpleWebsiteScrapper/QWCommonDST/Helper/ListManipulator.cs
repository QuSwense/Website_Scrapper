using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWCommonDST.Helper
{
    /// <summary>
    /// A List helper class which contains helper methods to enhance List class.
    /// It also can contain Extension methods
    /// </summary>
    public static class ListManipulator
    {
        /// <summary>
        /// Check if list is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
            => (list == null || list.Count <= 0);

        /// <summary>
        /// A common Looping algorithm call for all lists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="actionMethod"></param>
        public static void ForLoop<T>(this IList<T> list, Action<T, int> actionMethod)
        {
            if(list != null)
            {
                for(int indx = 0; indx < list.Count; ++indx)
                {
                    actionMethod(list[indx], indx);
                }
            }
        }
    }
}
