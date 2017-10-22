using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DynamicDatabase
{
    public class DynamicDbConnection
    {
        public string DataSource { get; protected set; }
        public string Database { get; protected set; }
        public int ConnectionTimeout { get; protected set; }
        public string ConnectionString { get; set; }
        public string Version { get; protected set; }
        public bool Pooling { get; protected set; }

        public DynamicDbConnection(string connection)
        {
            Parse(connection);
        }

        private void Parse(string connection)
        {
            Dictionary<string, string> connStringParts = connection.Split(';')
                    .Select(t => t.Split(new char[] { '=' }, 2))
                    .ToDictionary(t => t[0].Trim(), t => t[1].Trim(), 
                    StringComparer.InvariantCultureIgnoreCase);

            DataSource = connStringParts["Data Source"];
            Database = connStringParts["Database"];
            Version = connStringParts["Version"];
            Pooling = Convert.ToBoolean(connStringParts["Pooling"]);
        }
    }
}
