using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCommon.Error
{
    public class DynamicReaderException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            STORE_NULL
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }
        
        /// <summary>
        /// Default
        /// </summary>
        public DynamicReaderException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public DynamicReaderException(EErrorType type) : base(Initialize(type))
        {
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(EErrorType type)
        {
            switch (type)
            {
                case EErrorType.STORE_NULL:
                    return string.Format("The store object is null");
                default:
                    return "Unknwon Dynamic Reader error";
            }
        }
    }
}
