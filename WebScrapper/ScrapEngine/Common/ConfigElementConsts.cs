using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Common
{
    /// <summary>
    /// This static class defines the constants used in Config element processing
    /// </summary>
    public static class ConfigElementConsts
    {
        #region Column

        public const bool IsUniqueColumnDefault = false;
        public const int IndexColumnDefault = 0;
        public const bool ValueAsInnerHtmlColumnDefault = false;
        public const bool IsWhitespaceColumnDefault = false;
        public const bool DoUpdateOnlyColumnDefault = false;

        #endregion Column

        #region Manipulate

        public const bool IsEmptyOrNullColumnDefault = false;
        public const bool SplitAsStringColumnDefault = false;

        #endregion Manipulate
    }
}
