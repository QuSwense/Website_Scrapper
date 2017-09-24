namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// This class currently holds user input data which should be used in addition to (or in place of)
    /// other manipulative data.
    /// </summary>
    public class UserDataProcessorNode : ProcessorNode<UserDataProcessorNode>
    {
        /// <summary>
        /// A text which is used as a data value
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// A memebr method to clone data to 'this' node from another node
        /// </summary>
        /// <param name="node">The node from which to create the copy</param>
        /// <returns></returns>
        public override UserDataProcessorNode Clone(UserDataProcessorNode node)
        {
            Text = node.Text;

            return this;
        }
    }
}
