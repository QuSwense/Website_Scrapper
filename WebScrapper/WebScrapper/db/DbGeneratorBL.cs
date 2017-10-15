using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.scrap;

namespace WebScrapper.db
{
    public abstract class DbGeneratorBL
    {
        public DbConfigModel DbConfig { get; protected set; }

        public DbGeneratorBL(DbConfigModel dbconfig)
        {
            DbConfig = dbconfig;
        }

        public virtual void Generate()
        {
            CreateDbFile();

            try
            {
                OpenConnection();
                GenerateTables();
            }
            finally
            {
                CloseConnection();
            }
        }

        protected virtual void GenerateTables()
        {
            throw new NotImplementedException();
        }

        public virtual void CloseConnection()
        {
            throw new NotImplementedException();
        }

        public virtual void OpenConnection()
        {
            throw new NotImplementedException();
        }

        protected virtual void CreateDbFile()
        {
            throw new NotImplementedException();
        }

        public virtual void AddRow(string name, Dictionary<string, ColumnScrapModel> rowData)
        {
            throw new NotImplementedException();
        }
    }
}
