using BCMS_Backend.Settings;
using Microsoft.Data.Sqlite;

namespace BCMS_Backend.Repository
{
    /// <summary>
    /// it is mostly static class to ensure that in-memory storage stays always available
    /// </summary>
    public class InMemoryDatabase
    {
        /// <summary>
        /// pretty risky method to keep connection to SQLite in-memory database
        /// </summary>
        private static SqliteConnection keptConnection;
        public static void startupInMemoryDatabase()
        {
            var connectionString = GlobalSettings.GetConnectionConnectionString();
            keptConnection = new SqliteConnection(connectionString);
            keptConnection.Open();
            // nothing more to do, keptConnection is left hanging within this process
        }
        public static void finalizeInMemoryDatabase()
        {
            // in C++ I would be getting access violations for such move. C# is more forgiving. Lets hope that keptConnection will be closed
            // if not, it will be memory leak (in theory)
            keptConnection.Dispose();
        }
    }
}
