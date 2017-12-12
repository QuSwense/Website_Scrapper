using System;

namespace WebCommon.Error
{
    public class XmlReaderException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            ATTRIBUTE_VALUE_NULL,
            XML_ATTRIBUTE_NOT_MAPPED,
            NO_LIST_TYPE_MULTIPLE_ELEMENT
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }
        
        /// <summary>
        /// Default
        /// </summary>
        public XmlReaderException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public XmlReaderException(EErrorType type, params string [] args) : base(Initialize(type, args))
        {
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(EErrorType type, params string[] args)
        {
            switch (type)
            {
                case EErrorType.ATTRIBUTE_VALUE_NULL:
                    return string.Format("The value for the attribute '{0}' cannot be null", args);
                case EErrorType.XML_ATTRIBUTE_NOT_MAPPED:
                    return string.Format("The xml attribute '{0}' is not mapped", args);
                case EErrorType.NO_LIST_TYPE_MULTIPLE_ELEMENT:
                    return string.Format("The xml element '{0}' occurs more than once but the Property defined is not List type", args);
                default:
                    return "Unknwon Xml document reader exception error";
            }
        }
    }
}
