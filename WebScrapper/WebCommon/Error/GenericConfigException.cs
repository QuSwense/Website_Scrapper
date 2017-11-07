namespace WebCommon.Error
{
    public class GenericConfigException : ConfigException
    {
        public GenericConfigException() { }
        public GenericConfigException(string message) : base(message) { }
    }
}
