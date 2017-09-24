using System.Collections.Generic;

namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// Represents a generic template class which is a list of type <see cref="ScrapBaseProcessorNode"/>
    /// It is used to provide some common methods which is used to create and manioulate these
    /// type of lists
    /// The generic type <see cref="T"/> is a class of type <see cref="ScrapBaseProcessorNode"/>
    /// </summary>
    public class ProcessorNodeList<T> : List<T>, IProcessorNode where T: ScrapBaseProcessorNode, new()
    {
        /// <summary>
        /// Create a new instance of the list
        /// </summary>
        /// <returns></returns>
        public static ProcessorNodeList<T> New() => new ProcessorNodeList<T>();

        /// <summary>
        /// Create a new item and add to the list
        /// </summary>
        /// <returns>return the newly created item</returns>
        public T AddItem(string id = null)
        {
            T item = new T();
            item.Parent = this;
            if(!string.IsNullOrEmpty(id)) item.Id = id;
            Add(item);
            return item;
        }
    }
}
