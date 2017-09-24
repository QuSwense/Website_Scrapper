namespace SimpleWebsiteScrapper
{
    public static class GlobalSettings
    {
        public enum EDownloadMode
        {
            ONLINE,
            OFFLINE_STRICT,
            OFFLINE_THEN_ONLINE
        }
        public static EDownloadMode DownloadMode = EDownloadMode.ONLINE;
    }
}
