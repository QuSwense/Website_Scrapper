using System;

namespace ScrapException
{
    /// <summary>
    /// The exception class used to throw error from inside Combinatorics type classes
    /// </summary>
    public class CombinatoricsException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            ARGUMENT_NULL_OR_EMPTY
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }

        /// <summary>
        /// The path that has error
        /// </summary>
        public string[] Args { get; protected set; }

        /// <summary>
        /// Default
        /// </summary>
        public CombinatoricsException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public CombinatoricsException(EErrorType type, params string[] args) : base(Initialize(args, type))
        {
            Args = args;
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(string[] args, EErrorType type)
        {
            switch (type)
            {
                case EErrorType.ARGUMENT_NULL_OR_EMPTY:
                    return string.Format("The arguments passed {0} is null or empty", string.Join(" ", args));
                default:
                    return "Unknwon Combinatorics error";
            }
        }
    }
}
