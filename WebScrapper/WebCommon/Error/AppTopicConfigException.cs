namespace WebCommon.Error
{
    public class AppTopicConfigException : ConfigException
    {
        public AppTopicConfigException() { }
        public AppTopicConfigException(string message) : base(message) { }
    }
}
