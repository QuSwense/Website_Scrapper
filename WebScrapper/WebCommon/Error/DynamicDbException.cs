using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCommon.Error
{
    public class DynamicDbException : Exception
    {
        /// <summary>
        /// The type of path error
        /// </summary>
        public enum EErrorType
        {
            INIT_ARG_NULL,
            TABLE_NOT_FOUND,
            DUPLICATE_ROW,
            TABLE_LOADED,
            COLUMN_NOT_FOUND,
            COLUMN_INDEX_INVALID,
            UKEY_NULL_EMPTY
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
        public DynamicDbException() : base() { }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public DynamicDbException(EErrorType type) : base(Initialize(null, type))
        {
            ErrorType = type;
        }

        /// <summary>
        /// Constructor with path value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public DynamicDbException(EErrorType type, params string[] dataList) 
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
                case EErrorType.TABLE_NOT_FOUND:
                    return string.Format("The table {0} is not created or loaded", dataList[0]);
                case EErrorType.INIT_ARG_NULL:
                    return "The argument passed to the Initialize method is null";
                case EErrorType.DUPLICATE_ROW:
                    return string.Format("The table {0} has duplicate row for key {1}", dataList[0], dataList[1]);
                case EErrorType.TABLE_LOADED:
                    return string.Format("The table {0} is already loaded", dataList[0]);
                case EErrorType.COLUMN_NOT_FOUND:
                    return string.Format("The column {0} is not present", dataList[0]);
                case EErrorType.COLUMN_INDEX_INVALID:
                    return string.Format("The column index is invalid for column {0}", dataList[0]);
                case EErrorType.UKEY_NULL_EMPTY:
                    return string.Format("The unique key string cannot be null or empty for table {0}", dataList[0]);
                default:
                    return "Unknwon database error";
            }
        }
    }
}
