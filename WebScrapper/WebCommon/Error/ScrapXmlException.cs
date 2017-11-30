using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCommon.Error
{
    public class ScrapXmlException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            NODE_NOT_FOUND,
            INVALID_MANIPULATE_CHILD_ITEM,
            PARSE_NODE_ERROR,
            MANDATORY_ATTRIBUTE_NOT_FOUND,
            UNKNOWN_NODE,
            NODE_ATTRIBUTE_VALUE_NOT_FOUND,
            CHILD_NODE_PER_NODE_COUNT
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
        public ScrapXmlException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public ScrapXmlException(EErrorType type) : base(Initialize(null, type))
        {
            ErrorType = type;
        }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public ScrapXmlException(EErrorType type, params string[] dataList) 
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
                case EErrorType.INVALID_MANIPULATE_CHILD_ITEM:
                    return string.Format("The element tag {0} is invalid Manipulate child item tag", dataList);
                case EErrorType.NODE_NOT_FOUND:
                    return string.Format("The {0} node not found", dataList);
                case EErrorType.PARSE_NODE_ERROR:
                    return string.Format("The {0} node parser returned null object", dataList);
                case EErrorType.MANDATORY_ATTRIBUTE_NOT_FOUND:
                    return string.Format("The {0} node does not contain mandatory attribute {1}", dataList);
                case EErrorType.UNKNOWN_NODE:
                    return string.Format("The {0} node is not valid for the scrap xml file", dataList[0]);
                case EErrorType.NODE_ATTRIBUTE_VALUE_NOT_FOUND:
                    return string.Format("The {0} node must have {1} attribute", dataList);
                case EErrorType.CHILD_NODE_PER_NODE_COUNT:
                    return string.Format("Currently only {0} child {1} node per {2} node is supported", dataList);
                default:
                    return "Unknwon Web Scrap Parser error";
            }
        }
    }
}
