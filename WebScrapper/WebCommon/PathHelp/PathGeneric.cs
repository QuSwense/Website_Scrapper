using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebCommon.PathHelp
{
    /// <summary>
    /// A common path helper method
    /// </summary>
    public class PathGeneric
    {
        /// <summary>
        /// Get the parent path of this sub path
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
        /// Get the full path of the current item
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
                        _fullPath = Path.Combine(AppGenericConfigPathHelper.I.RootPath, Name);
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

        /// <summary>
        /// A parameterized constructor
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="parent"></param>
        public PathGeneric(string name, PathGeneric parent = null, bool isFile = false)
        {
            Name = name;
            Parent = parent;
            IsFile = isFile;
        }
    }
}
