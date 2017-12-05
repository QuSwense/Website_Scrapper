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
    /// <summary>
    /// The main database context class for Sqlite database interactions
    /// </summary>
    public class DatabaseContext
    {
        /// <summary>
        /// The connection object to connect Sqlite database
        /// </summary>
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

        /// <summary>
        /// Create a table from the Table metadata information
        /// This method is used to create a table dynamically
        /// </summary>
        /// <param name="tableColumnConfigs"></param>
        public void Create(DbTablesDefinitionModel tableColumnConfigs)
        {
            CreateTableQuery createTableQueryObj = new CreateTableQuery();
            createTableQueryObj.Generate(tableColumnConfigs);

            foreach (var sqlQuery in createTableQueryObj.SQLs)
            {
                ExecuteDML(sqlQuery);
            }
        }
        
        /// <summary>
        /// Add or Update a row of data into a given table
        /// </summary>
        /// <param name="name">The name of the table</param>
        /// <param name="row"></param>
        /// <param name="doUpdateOnly">If this is true then only update and no insert</param>
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

        public void AddPerformance(string key, Dictionary<string, TimeSpan> elapsedDataList, string type)
        {
            InsertOrUpdateIntoTableQuery commandQuery = new InsertOrUpdateIntoTableQuery();
            commandQuery.Generate(key, elapsedDataList, type);
            ExecuteDDL(commandQuery.SQL);
        }

        public void AddPerformance(string key, List<TimeSpan> totalElapsedList, string type)
        {
            InsertOrUpdateIntoTableQuery commandQuery = new InsertOrUpdateIntoTableQuery();
            commandQuery.Generate(key, totalElapsedList, type);
            ExecuteDDL(commandQuery.SQL);
        }

        public void AddTableMetadata(DbTablesMetdataDefinitionModel tableMetadatas)
        {
            InsertOrUpdateIntoTableQuery commandQuery = new InsertOrUpdateIntoTableQuery();
            commandQuery.Generate(tableMetadatas);
            ExecuteDDL(commandQuery.SQL);
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

        public int ValidateExists(string table, string column, string value)
        {
            SelectTableQuery selectUidObj = new SelectTableQuery();
            selectUidObj.GenerateValidation(table, column, value);

            SQLiteDataReader selectReader = ExecuteDML(selectUidObj.SQL);

            if(selectReader.Read())
            {
                return selectReader.GetInt32(0);
            }

            return -1;
        }

        public string SelectSingle(string selectQueryFormat, string result)
        {
            SelectTableQuery selectObj = new SelectTableQuery();
            selectObj.GenerateQueryFromFormat(selectQueryFormat, result);

            SQLiteDataReader selectReader = ExecuteDML(selectObj.SQL);

            if (selectReader.Read())
            {
                return selectReader.GetString(0);
            }

            return string.Empty;
        }
    }
}
