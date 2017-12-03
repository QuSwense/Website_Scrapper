using System;

namespace WebCommon.Error
{
    public class XmlDocReaderException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            ATTRIBUTE_VALUE_NULL,
            XML_ATTRIBUTE_NOT_MAPPED
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }
        
        /// <summary>
        /// Default
        /// </summary>
        public XmlDocReaderException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public XmlDocReaderException(EErrorType type, params string [] args) : base(Initialize(args, type))
        {
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(string [] args, EErrorType type)
        {
            switch (type)
            {
                case EErrorType.ATTRIBUTE_VALUE_NULL:
                    return string.Format("The value for the attribute '{0}' cannot be null", args);
                case EErrorType.XML_ATTRIBUTE_NOT_MAPPED:
                    return string.Format("The xml attribute '{0}' is not mapped", args);
                default:
                    return "Unknwon Xml document reader exception error";
            }
        }
    }
}
