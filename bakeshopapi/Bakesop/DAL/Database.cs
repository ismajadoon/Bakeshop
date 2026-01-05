using Microsoft.Data.Sqlite;

namespace Bakesop.DAL
{
    public class Database
    {
        public static string ConnectionString =
            "Data Source=Data/cakeshop.db";

        public static SqliteConnection GetConnection()
        {
            var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
