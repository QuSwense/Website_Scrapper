using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCommon.Error
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
            SCRAP_NAME_EMPTY
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
                    return string.Format("The level {0} of Scrap tag is invalid", dataList[0]);
                case EErrorType.SCRAP_NAME_MULTIPLE:
                    return string.Format("The Scrap child tags may contain multiple name attribute which is not allowed");
                case EErrorType.SCRAP_NAME_EMPTY:
                    return string.Format("The Scrap name attribute cannot be empty. It must be present at in one of the childs");
                default:
                    return "Unknwon Web Scrap Parser error";
            }
        }
    }
}
