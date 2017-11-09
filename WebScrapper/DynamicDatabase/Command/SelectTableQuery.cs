using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WebCommon.Extn;

namespace DynamicDatabase.Command
{
    public class SelectTableQuery : ICommandQuery
    {
        #region Properties

        /// <summary>
        /// The parent DbCommand class
        /// </summary>
        private IDbCommand dbCommand;

        /// <summary>
        /// Column definition list
        /// </summary>
        private List<string> columnDefinitions;
        
        /// <summary>
        /// The SQL statement
        /// </summary>
        public List<string> SQLs { get; protected set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The constructor with command class
        /// </summary>
        public SelectTableQuery(IDbCommand dbCommand)
        {
            this.dbCommand = dbCommand;
            columnDefinitions = new List<string>();
            SQLs = new List<string>();
        }

        #endregion Constructor

        #region Generate

        /// <summary>
        /// Start the sql generation
        /// </summary>
        public void GenerateAll(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            SQLs.Add(dbCommand.SelectQueryString.Inject(
                new Dictionary<string, string>()
                {
                    { "Columns", "*" },
                    { "TableName",  name }
                }));
        }

        #endregion Generate
    }
}
