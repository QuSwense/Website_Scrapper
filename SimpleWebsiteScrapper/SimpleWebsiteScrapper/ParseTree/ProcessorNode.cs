using System;

namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// A base abstract class for all other processor nodes
    /// </summary>
    public abstract class ProcessorNode<T> : IProcessorNode where T: ProcessorNode<T>
    {
        /// <summary>
        /// The property which refers to the Parent node to which it belongs
        /// It can be of type <see cref="ProcessorNode{T}"/> or
        /// <see cref="ProcessorNodeList{T}"/>
        /// </summary>
        public IProcessorNode Parent { get; set; }

        /// <summary>
        /// A unique identifier to identify the processor node
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A static method create an instance of a <see cref="ProcessorNode"/> type.
        /// An inherited class shall create same method with <code>new</code> keyword
        /// </summary>
        /// <returns></returns>
        public static T New() => null;

        /// <summary>
        /// A virtual method to clone an instance of <see cref="ProcessorNode"/> type to
        /// the <code>this</code> instance of the class.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual T Clone(T node) { return (T)this; }

        /// <summary>
        /// A static method copy and create an instance of a <see cref="ProcessorNode"/> type.
        /// An inherited class shall create same method with <code>new</code> keyword
        /// </summary>
        /// <returns></returns>
        public static T CloneNew(T node) => null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcessorNode()
        {
            Id = GetUniqueName("");
        }

        /// <summary>
        /// A unique name identifier generator using <see cref="Guid"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetUniqueName(string name)
            => (string.IsNullOrEmpty(name)) ? Guid.NewGuid().ToString() : name;
    }
}
