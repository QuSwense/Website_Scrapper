﻿using System;

namespace WebCommon.Error
{
    public class CommandLineException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            PARSE_ERROR
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
        public CommandLineException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public CommandLineException(EErrorType type, params string[] args) : base(Initialize(args, type))
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
                case EErrorType.PARSE_ERROR:
                    return string.Format("The arguments '{0}' to the program are not proper", string.Join(" ", args));
                default:
                    return "Unknwon Command line error";
            }
        }
    }
}
