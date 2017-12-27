using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.Scrap
{
    /// <summary>
    /// The base class for all the config element
    /// </summary>
    public class ConfigElementBase : IConfigElement
    {
        #region IConfigElement Implementation

        /// <summary>
        /// Get the Id of the scrap config unit.
        /// If the current element is child element then find the id defined at the parent level 
        /// recursively
        /// </summary>
        public virtual string IdScrapUnit
        {
            get { return ScrapParent.IdScrapUnit; }
        }

        /// <summary>
        /// Get the Id of the scrap config element.
        /// It recursively calculates the level id and sibling id and appends to the main
        /// element
        /// </summary>
        public virtual string IdCurrent
        {
            get { return string.Format("{0}_{1}_{2}", IdScrapUnit, LevelInScrapUnit, SiblingIndex); }
        }

        /// <summary>
        /// The name of the column
        /// </summary>
        [DXmlAttribute(ScrapXmlConsts.NameAttributeName)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Get the level of the current node in the scrap unit tree
        /// </summary>
        public virtual int LevelInScrapUnit
        {
            get
            {
                return ParentConfig == null ? 0 : ParentConfig.LevelInScrapUnit + 1;
            }
        }

        /// <summary>
        /// Get the sibling index of the current node under its parent
        /// </summary>
        public virtual int SiblingIndex
        {
            get
            {
                if (ParentConfig == null) return 0;
                else
                    return ParentConfig.Children.FindIndex(cfg => ReferenceEquals(this, cfg));
            }
        }

        /// <summary>
        /// Return the parent config element of the current config element
        /// </summary>
        [DXmlParent]
        public IConfigElement ParentConfig { get; set; }

        /// <summary>
        /// A set of child elements in order of occurance
        /// </summary>
        public virtual List<IConfigElement> Children { get; set; }

        #endregion IConfigElement Implementation

        #region Helper Methods

        /// <summary>
        /// Get the immediate Parent of this current column node which is a Scrap Element node
        /// </summary>
        /// <returns></returns>
        public IConfigElement ScrapParent
        {
            get
            {
                IConfigElement iterElement = this;
                while ((iterElement != null) &&
                    !(iterElement is ScrapElement)) iterElement = iterElement.ParentConfig;

                Debug.Assert(iterElement != null);

                return iterElement;
            }
        }

        #endregion Helper Methods
    }
}
