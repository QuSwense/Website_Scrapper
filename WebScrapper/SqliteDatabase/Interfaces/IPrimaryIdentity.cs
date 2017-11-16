namespace SqliteDatabase.Interfaces
{
    /// <summary>
    /// A common interface which contains identity informations to identify the class
    /// </summary>
    public interface IPrimaryIdentity
    {
        /// <summary>
        /// A name to display
        /// </summary>
        string Name { get; }
    }
}
