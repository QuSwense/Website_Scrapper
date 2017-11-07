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
            ATRRIBUTE_VALUE_NULL
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }

        /// <summary>
        /// The path that has error
        /// </summary>
        public string AttributeName { get; protected set; }

        /// <summary>
        /// Default
        /// </summary>
        public XmlDocReaderException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public XmlDocReaderException(string attrName, EErrorType type) : base(Initialize(attrName, type))
        {
            AttributeName = attrName;
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(string attrName, EErrorType type)
        {
            switch (type)
            {
                case EErrorType.ATRRIBUTE_VALUE_NULL:
                    return string.Format("The value for the attribute '{0}' cannot be null", attrName);
                default:
                    return "Unknwon Xml document reader exception error";
            }
        }
    }
}
