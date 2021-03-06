﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrapException
{
    public class ScrapParserException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            SCRAP_LEVEL_INVALID,
            SCRAP_NAME_MULTIPLE,
            SCRAP_NAME_EMPTY,
            UNKNOWN_MANIPULATE_CHILD_TYPE,
            MANIPULATE_TRIM_DATA_EMPTY,
            MANIPULATE_SPLIT_DATA_EMPTY,
            MANIPULATE_SPLIT_INDEX_INVALID,
            MANIPULATE_REGEX_DATA_EMPTY,
            MANIPULATE_REGEX_INDEX_INVALID,
            MANIPULATE_REPLACE_INSTRING_EMPTY,
            SCRAP_INDEX_INVALID,
            NOT_IMPLEMENTED,
            VALIDATION_FAILED,
            DUPLICATE_PERFORMANCE_CONFIG
        }

        /// <summary>
        /// The type of error
        /// </summary>
        public EErrorType ErrorType { get; protected set; }

        /// <summary>
        /// The path that has error
        /// </summary>
        public List<string> DataList { get; protected set; }

        /// <summary>
        /// Default
        /// </summary>
        public ScrapParserException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public ScrapParserException(EErrorType type) : base(Initialize(null, type))
        {
            ErrorType = type;
        }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public ScrapParserException(EErrorType type, params string[] dataList) 
            : base(Initialize(dataList.ToList(), type))
        {
            DataList = dataList.ToList();
            ErrorType = type;
        }

        /// <summary>
        /// Initialize message
        /// </summary>
        private static string Initialize(List<string> dataList, EErrorType type)
        {
            switch (type)
            {
                case EErrorType.SCRAP_LEVEL_INVALID:
                    return string.Format("The level {0} of Scrap tag is invalid", dataList);
                case EErrorType.SCRAP_NAME_MULTIPLE:
                    return string.Format("The Scrap child tags may contain multiple name attribute which is not allowed");
                case EErrorType.SCRAP_NAME_EMPTY:
                    return string.Format("The Scrap name attribute cannot be empty. It must be present at in one of the childs");
                case EErrorType.UNKNOWN_MANIPULATE_CHILD_TYPE:
                    return string.Format("Unknown manipulate child element {0}", dataList);
                case EErrorType.MANIPULATE_TRIM_DATA_EMPTY:
                    return string.Format("The Manipulate Trim node has empty Data attribute {0}", dataList);
                case EErrorType.MANIPULATE_SPLIT_DATA_EMPTY:
                    return string.Format("The Manipulate Split node has empty Data attribute {0}", dataList);
                case EErrorType.MANIPULATE_SPLIT_INDEX_INVALID:
                    return string.Format("The Manipulate Split node has invalid index attribute {0}", dataList);
                case EErrorType.MANIPULATE_REGEX_DATA_EMPTY:
                    return string.Format("The Manipulate Regex node has empty Data attribute {0}", dataList);
                case EErrorType.MANIPULATE_REGEX_INDEX_INVALID:
                    return string.Format("The Manipulate Regex node has invalid index attribute {0}", dataList);
                case EErrorType.MANIPULATE_REPLACE_INSTRING_EMPTY:
                    return string.Format("The Manipulate Replace node has invalid In String attribute {0}", dataList);
                case EErrorType.SCRAP_INDEX_INVALID:
                    return string.Format("The index of the Scrap element is invalid");
                case EErrorType.NOT_IMPLEMENTED:
                    return string.Format("The Config element type is not implemented");
                case EErrorType.VALIDATION_FAILED:
                    return string.Format("The value '{0}' is not found in table '{1}' and '{2}'");
                case EErrorType.DUPLICATE_PERFORMANCE_CONFIG:
                    return string.Format("The config element {0} is being processed twice.", dataList);
                default:
                    return "Unknwon Web Scrap Parser error";
            }
        }
    }
}
