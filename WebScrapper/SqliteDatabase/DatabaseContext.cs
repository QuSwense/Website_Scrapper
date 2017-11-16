using SqliteDatabase.Command;
using SqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Linq;

namespace SqliteDatabase
{
    public class DatabaseContext
    {
        public SQLiteConnection Connection { get; protected set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string AppTopic { get; protected set; }

        /// <summary>
        /// The full local path of the database file
        /// </summary>
        public string FullPath { get; protected set; }

        public string DbFullPath
        {
            get { return Path.Combine(FullPath, AppTopic); }
        }

        /// <summary>
        /// Initialize the database connection
        /// </summary>
        /// <param name="dbfilepath"></param>
        /// <param name="name"></param>
        public void Initialize(string dbfilepath, string appTopic)
        {
            FullPath = dbfilepath;
            AppTopic = appTopic;

            Connection = new SQLiteConnection(string.Format("Data Source={0}.sqlite",
                Path.Combine(FullPath, AppTopic)));
        }

        /// <summary>
        /// An method whose purpose is to create the database
        /// </summary>
        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DbFullPath + ".sqlite");
        }

        /// <summary>
        /// An method whose purpose is to check if the database already exists or not
        /// </summary>
        /// <returns></returns>
        public bool DatabaseExists()
        {
            return File.Exists(DbFullPath);
        }

        /// <summary>
        /// An method whose purpose is to delete the database
        /// </summary>
        public void DeleteDatabase()
        {
            File.Delete(DbFullPath);
        }

        /// <summary>
        /// Open a connection
        /// </summary>
        public virtual void Open()
        {
            Connection.Open();
        }

        /// <summary>
        /// Close a connection
        /// </summary>
        public virtual void Close()
        {
            Connection.Close();
        }

        protected void CreateTableMetadata()
        {
            StringBuilder SQL = new StringBuilder("CREATE TABLE IF NOT EXISTS mdt (");
            SQL.Append("tblnm TEXT,");
            SQL.Append("dspnm TEXT,");
            SQL.Append("dsc TEXT,");
            SQL.Append(" PRIMARY KEY(tblnm))");

            ExecuteDDL(SQL.ToString());
        }

        public void Create(DbTablesDefinitionModel tableColumnConfigs)
        {
            CreateTableQuery createTableQueryObj = new CreateTableQuery();
            createTableQueryObj.Generate(tableColumnConfigs);

            foreach (var sqlQuery in createTableQueryObj.SQLs)
            {
                ExecuteDML(sqlQuery);
            }
        }

        protected void CreateTableScrapMetadata()
        {
            StringBuilder SQL = new StringBuilder("CREATE TABLE IF NOT EXISTS tblscrpmdt (");
            SQL.Append("uid INTEGER,");
            SQL.Append("nm TEXT,");
            SQL.Append("url1 TEXT,");
            SQL.Append("url2 TEXT,");
            SQL.Append("url3 TEXT,");
            SQL.Append("url4 TEXT,");
            SQL.Append("xpath1 TEXT,");
            SQL.Append("xpath2 TEXT,");
            SQL.Append("xpath3 TEXT,");
            SQL.Append("xpath4 TEXT,");
            SQL.Append(" PRIMARY KEY(uid, nm))");

            ExecuteDDL(SQL.ToString());
        }

        public void AddOrUpdate(string name, List<DynamicTableDataInsertModel> row, bool doUpdateOnly = false)
        {
            SelectTableQuery selectUniqueRowObj = new SelectTableQuery();
            selectUniqueRowObj.GenerateUniqueRow(name, row);

            List<ColumnModel> colValueMappings = new List<ColumnModel>();
            if (!string.IsNullOrEmpty(selectUniqueRowObj.SQL))
            {
                SQLiteDataReader selectUniqueRowReader = ExecuteDML(selectUniqueRowObj.SQL);
                int rowCount = 0;
                if (selectUniqueRowReader.HasRows)
                {
                    while (selectUniqueRowReader.Read())
                    {
                        if (rowCount == 1)
                            throw new Exception(
                                string.Format("Multiple Rows found for the Unique query : {0}", selectUniqueRowObj.SQL));

                        for (int i = 0; i < selectUniqueRowReader.FieldCount; i++)
                        {
                            ColumnModel colModel = new ColumnModel();
                            colModel.Name = selectUniqueRowReader.GetName(i);
                            colModel.DataType = DataTypeContextHelper.GetType(selectUniqueRowReader.GetFieldType(i));

                            // Find the columns
                            DynamicTableDataInsertModel colInsertObj = row.Where(p => p.Name == colModel.Name).FirstOrDefault();

                            if (colInsertObj == null)
                                colModel.Value = selectUniqueRowReader.GetValue(i);
                            else
                                colModel.Value = colInsertObj.Value;

                            colValueMappings.Add(colModel);
                        }

                        rowCount++;
                    }
                }
            }

            InsertOrUpdateIntoTableQuery commandQuery = new InsertOrUpdateIntoTableQuery();

            if (colValueMappings.Count > 1)
                commandQuery.Generate(name, colValueMappings);
            else if(!doUpdateOnly)
                commandQuery.Generate(name, row);

            if(!string.IsNullOrEmpty(commandQuery.SQL))
                ExecuteDDL(commandQuery.SQL);
        }

        public void AddTableMetadata(DbTablesMetdataDefinitionModel tableMetadatas)
        {
            InsertOrUpdateIntoTableQuery commandQuery = new InsertOrUpdateIntoTableQuery();
            commandQuery.Generate(tableMetadatas);
            ExecuteDDL(commandQuery.SQL);
        }

        protected void CreateColumnScrapMetadata()
        {
            StringBuilder SQL = new StringBuilder("CREATE TABLE IF NOT EXISTS colscrpmdt (");
            SQL.Append("colm TEXT,");
            SQL.Append("dspnm TEXT,");
            SQL.Append("dsc TEXT,");
            SQL.Append("xpath TEXT,");
            SQL.Append("indx INTEGER,");
            SQL.Append("uid INTEGER,");
            SQL.Append(" PRIMARY KEY(colm, uid))");

            ExecuteDDL(SQL.ToString());
        }

        public void CreateMetadata()
        {
            CreateTableMetadata();
            CreateTableScrapMetadata();
            CreateColumnScrapMetadata();
        }

        /// <summary>
        /// Execute Data Definiton Language
        /// </summary>
        private void ExecuteDDL(string SQL)
        {
            using (SQLiteCommand fmd = new SQLiteCommand(SQL, Connection))
                fmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute the Data manipulation query and return the reader
        /// </summary>
        /// <returns></returns>
        public SQLiteDataReader ExecuteDML(string SQL)
        {
            using (SQLiteCommand fmd = new SQLiteCommand(SQL, Connection))
            {
                fmd.CommandType = CommandType.Text;
                return fmd.ExecuteReader();
            }
        }

        public void Add(List<ColumnScrapMetadataModel> colScrapMdtModels)
        {
            InsertOrUpdateIntoTableQuery commandQuery = new InsertOrUpdateIntoTableQuery();
            commandQuery.Generate(colScrapMdtModels);
            ExecuteDDL(commandQuery.SQL);
        }

        public int Add(TableScrapMetadataModel tblScrapMdtModel)
        {
            // Get the last UID
            SelectTableQuery selectUidObj = new SelectTableQuery();
            selectUidObj.GenerateTableScrapUid(tblScrapMdtModel.Name);

            SQLiteDataReader selectUidReader = ExecuteDML(selectUidObj.SQL);
            int lastUid = 0;
            if(selectUidReader.Read())
            {
                if(!selectUidReader.IsDBNull(0))
                    lastUid = selectUidReader.GetInt32(0);
            }

            InsertOrUpdateIntoTableQuery commandQuery = new InsertOrUpdateIntoTableQuery();
            commandQuery.Generate(tblScrapMdtModel, ++lastUid);
            ExecuteDDL(commandQuery.SQL);

            return lastUid;
        }
    }
}
