namespace SimpleWebsiteScrapper
{
    /// <summary>
    /// A class which maintains the global settings for the whole project
    /// </summary>
    public static class GlobalSettings
    {
        /// <summary>
        /// The type of download mode for the webpage
        /// </summary>
        public enum EDownloadMode
        {
            // Only use online resource
            ONLINE,

            // ONly use offline mode
            OFFLINE_STRICT,

            // First try offline, if failed then try online
            OFFLINE_THEN_ONLINE
        }

        /// <summary>
        /// The Download mode
        /// </summary>
        public static EDownloadMode DownloadMode = EDownloadMode.ONLINE;

        /// <summary>
        /// This is used by any class derived from the class <see cref="Output.ProcessorEngineOutput"/>
        /// to determine if the output generated is to be minimised or not
        /// </summary>
        public static bool UseMinimisedProcessorFileOutputMode = false;

        /// <summary>
        /// The seperator character(s) used
        /// </summary>
        public static char WhitespaceFileOutputMode = ' ';
    }
}
