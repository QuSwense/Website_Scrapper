using ScrapException;
using System.IO;

namespace ConfigPathHelper
{
    /// <summary>
    /// The Web Scrapper application is using lot of config files stored in folders and subfolders.
    /// To manage all those paths and files efficiently and smoothly this helper class is used.
    /// This class may represent a folder or a file.
    /// </summary>
    public class PathGeneric
    {
        #region Properties

        /// <summary>
        /// Get the parent folder path of this path
        /// </summary>
        public PathGeneric Parent { get; protected set; }

        /// <summary>
        /// A check to set if this is a file or not
        /// </summary>
        public bool IsFile { get; protected set; }

        /// <summary>
        /// The name of the folder or file
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The root folder path
        /// </summary>
        public string RootPath { get; protected set; }

        /// <summary>
        /// Get the full path of the current item
        /// This uses recursion to calculate the full path
        /// </summary>
        private string _fullPath;
        public string FullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_fullPath))
                {
                    if (Parent != null)
                        _fullPath = Path.Combine(Parent.FullPath, Name);
                    else
                        _fullPath = Path.Combine(RootPath, Name);
                }

                return _fullPath;
            }
        }

        /// <summary>
        /// Check if the folder or file exists
        /// </summary>
        public bool Exists
        {
            get
            {
                if (IsFile)
                    return File.Exists(FullPath);
                else
                    return Directory.Exists(FullPath);
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// A parameterized constructor
        /// </summary>
        /// <param name="name">The name of file or folder that this class represents</param>
        /// <param name="parent">The parent path</param>
        /// <param name="isFile">check to set if this is a file or folder</param>
        public PathGeneric(string name, PathGeneric parent = null, bool isFile = false)
        {
            Name = name;
            Parent = parent;
            IsFile = isFile;
        }

        /// <summary>
        /// A parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public PathGeneric(string name, string path)
        {
            Name = name;
            RootPath = path;
        }

        #endregion Constructor

        #region Asserts

        /// <summary>
        /// This method checks if the path exists or not.
        /// If it does not exists then throws exception
        /// </summary>
        public void AssertExists()
        {
            if (!Exists) throw new PathException(FullPath, PathException.EErrorType.NOT_EXISTS);
        }

        #endregion Static
    }
}
