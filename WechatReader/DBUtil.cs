using System.Collections.Generic;
using System.Data.SQLite;

namespace WechatReader
{
    static class DBUtil
    {
        public static SQLiteConnection Connect(string path)
        {
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = path,
                Version = 3,
            };
            var conn = new SQLiteConnection(builder.ConnectionString);
            conn.Open();
            return conn;
        }
    }

    static class SQLiteDataReaderExtension
    {
        public static string TryGetString(this SQLiteDataReader reader, int i)
        {
            if (reader.IsDBNull(i)) return null;
            return reader.GetString(i);
        }

        public static int TryGetInt(this SQLiteDataReader reader, int i)
        {
            if (reader.IsDBNull(i)) return 0;
            return reader.GetInt32(i);
        }
        public static long TryGetLong(this SQLiteDataReader reader, int i)
        {
            if (reader.IsDBNull(i)) return 0;
            return reader.GetInt64(i);
        }

        public static List<Section> TryGetBlobParse(this SQLiteDataReader reader, int i)
        {
            if (reader.IsDBNull(i)) return null;
            using (var stream = reader.GetStream(i))
            {
                return BlobParser.Parse(stream);
            }
        }
    }
}
