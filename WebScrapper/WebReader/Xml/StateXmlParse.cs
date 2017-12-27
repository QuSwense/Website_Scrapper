using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace WebReader.Xml
{
    /// <summary>
    /// An intermediate class which represents a state of the parser
    /// during xml parsing
    /// </summary>
    public class StateXmlParse
    {
        #region Properties

        /// <summary>
        /// The attribute instance defined on a property
        /// </summary>
        public DXmlElementAttribute ElemPropertyAttribute { get; set; }

        /// <summary>
        /// The current instance type. This needs to be stored to identify the difference
        /// with the Type actually meant and type that is parsed
        /// </summary>
        public Type InstanceType { get; set; }

        /// <summary>
        /// The current instance instance object
        /// </summary>
        public object InstanceObj { get; set; }

        /// <summary>
        /// The current property information object
        /// </summary>
        public PropertyInfo PropInfo { get; set; }

        /// <summary>
        /// This keeps a count of the number of times this instance is accessed
        /// </summary>
        public int Accessed { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public StateXmlParse()
        {
            Accessed = 0;
        }

        #endregion Constructor
    }
}
