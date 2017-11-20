using ScrapEngine.Bl;
using ScrapEngine.Bl.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCommon.Error;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// The base class for Manipulate child items
    /// This class is required to store the type of Child node information
    /// </summary>
    public class ManipulateChildElement
    {
        /// <summary>
        /// Type of Manupulate class type
        /// </summary>
        public enum EManipulateType
        {
            Split,
            Trim,
            Replace,
            Regex
        }

        /// <summary>
        /// The manipulate child object type
        /// </summary>
        protected EManipulateType manipulateType;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ManipulateChildElement() { }

        /// <summary>
        /// Create an instance of Manipulate child
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ManipulateChildElement Create(string name)
        {
            if (name == "Split") return new SplitElement();
            else if (name == "Trim") return new TrimElement();
            else if (name == "Regex") return new RegexElement();
            else if (name == "Replace") return new ReplaceElement();
            else
                throw new ScrapXmlException(ScrapXmlException.EErrorType.INVALID_MANIPULATE_CHILD_ITEM);
        }
    }
}
