using System;

namespace ScrapException
{
    /// <summary>
    /// Handles any error in the path.
    /// IT handles a file, folder types
    /// </summary>
    public class PathException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            NOT_EXISTS
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }

        /// <summary>
        /// The path that has error
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// Default
        /// </summary>
        public PathException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public PathException(string path, EErrorType type) : base(Initialize(path, type))
        {
            Path = path;
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(string path, EErrorType type)
        {
            switch (type)
            {
                case EErrorType.NOT_EXISTS:
                    return string.Format("The path {0} do not exists", path);
                default:
                    return "Unknwon path error";
            }
        }
    }
}
