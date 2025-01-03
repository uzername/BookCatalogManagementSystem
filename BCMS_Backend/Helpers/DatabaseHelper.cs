using BCMS_Backend.Settings;
using Microsoft.Data.Sqlite;

namespace BCMS_Backend.Helpers
{
    /// <summary>
    /// adapted from https://github.com/Unbalanced-Tree/CSharp/blob/main/InMemoryDb/InMemoryDb/Helpers/DbHelpers.cs
    /// </summary>
    public class DatabaseHelper
    {
        public static readonly string dbStructurePath = $@"{Environment.CurrentDirectory}\DatasourceStructure\BCMS_Initial.db3";
        public static readonly string dbBackupPath = @$"{Environment.CurrentDirectory}\localDB.db3";
        private static SqliteConnection inMemoryDbConnection = null;
        /// <summary>
        /// this provides connection object either to db3 file with initial structure or to db3 file of backup
        /// </summary>
        /// <param name="useStructurePath"></param>
        /// <returns></returns>
        public static SqliteConnection GetPhysicalDbConnection(bool useStructurePath)
        {
            string dbPath;
            if (useStructurePath)   {
                dbPath = dbStructurePath;
            }  else  {
                dbPath = dbBackupPath;
            }
            string physicalConnString = "Data Source =" + dbPath + ";Mode=ReadWrite";
            var dbConnection = new SqliteConnection(physicalConnString);
            
            dbConnection.Open();
            return dbConnection;
        }
        /// <summary>
        /// this provides connection object to in-memory database. It should not be closed and kept for process lifetime
        /// </summary>
        /// <returns></returns>
        public static SqliteConnection GetInMemoryDbConnection()
        {
            if (inMemoryDbConnection == null)
            {
                inMemoryDbConnection = new SqliteConnection(GlobalSettings.GetConnectionConnectionString());
                inMemoryDbConnection.Open();
                // initialize in-memory database structure from prepared db3 file (in-memory storage it will be empty on creation)
                var physicalDbConnection = DatabaseHelper.GetPhysicalDbConnection(true);
                physicalDbConnection.BackupDatabase(inMemoryDbConnection);
                physicalDbConnection.Close();

                return inMemoryDbConnection;
            }
            return inMemoryDbConnection;
        }
    }
}
