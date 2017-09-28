using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWSettings
{
    /// <summary>
    /// This is the Settings class used fo any Url settings data
    /// </summary>
    public class UrlHelperSettings : HelperSettings<UrlSettings>
    {
        /// <summary>
        /// The settings which specifies if Offline uri will be generated from online Uri if offline
        /// Uri is not present
        /// </summary>
        public static bool DoGenerateOfflineUriFromOnline
        {
            get
            {
                return Convert.ToBoolean(SettingsObj.DoGenerateOfflineUriFromOnline);
            }
        }

        public static bool UseOfflineLinkPreference
        {
            get
            {
                return Convert.ToBoolean(SettingsObj.UseOfflineLinkPreference);
            }
        }
    }
}
