using QWSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QWCommonDST.Helper
{
    /// <summary>
    /// A helper class to do all sorts of common string manipulations required
    /// It defines Extension method wherever possible for <see cref="string"/>
    /// </summary>
    public static class StringManipulator
    {
        /// <summary>
        /// If you need to use a string which may contain characters that are not valid for a path then
        /// use this method to sanitize the string value
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string PathVerifyAndClean(this string data, string replaceText = "_")
            => Regex.Replace(data, PathHelperSettings.RegexUnwantedPathChars, replaceText);
    }
}
