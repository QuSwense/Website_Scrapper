using System;

namespace ScrapException
{
    /// <summary>
    /// Any XPath node iterator
    /// </summary>
    public class HtmlNodeException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            XPATH_NODE_NULL
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }

        /// <summary>
        /// The path that has error
        /// </summary>
        public string XPath { get; protected set; }

        /// <summary>
        /// Default
        /// </summary>
        public HtmlNodeException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public HtmlNodeException(string xPath, EErrorType type) : base(Initialize(xPath, type))
        {
            XPath = xPath;
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(string xPath, EErrorType type)
        {
            switch (type)
            {
                case EErrorType.XPATH_NODE_NULL:
                    return string.Format("The node reference is null for the XPATH {0}", xPath);
                default:
                    return "Unknwon Html Node error";
            }
        }
    }
}
