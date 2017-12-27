namespace ScrapException
{
    public class GenericDbConfigException : GenericConfigException
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            NULL_STORE,
            EMPTY_STORE,
            MULTIPLE_TABLE_STORE
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }

        /// <summary>
        /// The path that has error
        /// </summary>
        public string ConfigStoreName { get; protected set; }

        /// <summary>
        /// Default
        /// </summary>
        public GenericDbConfigException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public GenericDbConfigException(string storeName, EErrorType type) : base(Initialize(storeName, type))
        {
            ConfigStoreName = storeName;
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(string storeName, EErrorType type)
        {
            switch (type)
            {
                case EErrorType.NULL_STORE:
                    return string.Format("The global database config data '{0}' cannot be null", storeName);
                case EErrorType.EMPTY_STORE:
                    return string.Format("The global database config data '{0}' cannot be empty", storeName);
                case EErrorType.MULTIPLE_TABLE_STORE:
                    return string.Format("The global database config data '{0}' does not support multiple tables", storeName);
                default:
                    return "Unknwon Command line error";
            }
        }
    }
}
