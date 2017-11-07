using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCommon.Error
{
    public class DynamicDbException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            TABLE_NOT_FOUND
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }

        /// <summary>
        /// The path that has error
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Default
        /// </summary>
        public DynamicDbException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public DynamicDbException(string name, EErrorType type) : base(Initialize(name, type))
        {
            Name = name;
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(string name, EErrorType type)
        {
            switch (type)
            {
                case EErrorType.TABLE_NOT_FOUND:
                    return string.Format("The table is not created or loaded", name);
                default:
                    return "Unknwon Command line error";
            }
        }
    }
}
