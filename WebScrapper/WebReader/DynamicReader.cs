using System;
using System.IO;

namespace WebReader
{
    /// <summary>
    /// The class is the base reader for reading any kind of file.
    /// This class contains basic functionalities to read a file.
    /// </summary>
    public abstract class DynamicReader : IDisposable
    {
        /// <summary>
        /// The full file path of the file. It can be a network file.
        /// </summary>
        public string FullPath { get; protected set; }

        /// <summary>
        /// The name of the file with extension
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// The object data store
        /// </summary>
        public object Store { get; protected set; }

        /// <summary>
        /// The type of the data store.
        /// Also set create the instance of the store
        /// </summary>
        public Type StoreType
        {
            get
            {
                return Store.GetType();
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamicReader() { }

        /// <summary>
        /// Constructor with file path
        /// </summary>
        /// <param name="fullfile"></param>
        public DynamicReader(string fullfile, object store)
        {
            FileInfo fi = new FileInfo(fullfile);
            FullPath = fi.Directory.FullName;
            FileName = fi.Name;
            Store = store;
        }
        
        /// <summary>
        /// The main virtual read function
        /// </summary>
        public virtual void Read()
        {
            if (Store == null) throw new Exception("The store object is null");

            using (var txtreader = new StreamReader(Path.Combine(FullPath, FileName)))
            {
                string line = null;
                while ((line = txtreader.ReadLine()) != null)
                {
                    ReadLineOverride(line);
                }
            }
        }

        /// <summary>
        /// This is a protected method which is overriden in derived class
        /// </summary>
        protected virtual void ReadLineOverride(string line) { }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Store = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
