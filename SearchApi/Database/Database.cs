using Npgsql;
using SearchApi.Models;

namespace SearchApi
{
    public class Database : IDatabase
    {
        private readonly NpgsqlConnection connection;
        
        public Database()
        {
            var connectionString = ConnectionStringBuilder.Create("postgres");

            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public Database(string connectionString)
        {
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public NpgsqlCommand CreateCommand(string sql)
        {
            return new NpgsqlCommand(sql, connection);
        }

        public List<KeyValuePair<int, int>> GetDocuments(List<int> wordIds)
        {
            var res = new List<KeyValuePair<int, int>>();

            // Create a parameterized SQL query
            var sql = "SELECT \"docId\", COUNT(\"wordId\") as count FROM \"occ\" WHERE \"wordId\" = ANY(@wordIds) GROUP BY \"docId\" ORDER BY COUNT(\"wordId\") DESC;";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                // Add a parameter for the wordIds list
                cmd.Parameters.AddWithValue("wordIds", wordIds.ToArray());

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var docId = reader.GetInt32(0);
                    var count = reader.GetInt32(1);

                    res.Add(new KeyValuePair<int, int>(docId, count));
                }
            }

            return res;
        }

        public Dictionary<string, int> GetAllWords()
        {
            var res = new Dictionary<string, int>();

            // Create a parameterized SQL query with explicit casting to 'text'
            var sql = "SELECT \"id\", \"word\"::text FROM \"word\"";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var word = reader.GetString(1);

                    word = ExtractWord(word);

                    res.Add(word, id);
                }
            }
            return res;
        }

        public List<BEDocument> GetDocDetails(List<int> docIds)
        {
            var res = new List<BEDocument>();

            // Create a parameterized SQL query
            var sql = "SELECT \"id\", \"url\", \"idxTime\", \"creationTime\" FROM \"document\" WHERE \"id\" = ANY(@docIds);";

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("docIds", docIds.ToArray());

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var url = reader.GetString(1);
                    var idxTime = reader.GetString(2);
                    var creationTime = reader.GetString(3);

                    res.Add(new BEDocument { Id = id, Url = url, IdxTime = idxTime, CreationTime = creationTime });
                }
            }
            return res;
        }

        public static string ExtractWord(string fullText)
        {
            int pFrom = fullText.IndexOf(",") + ",".Length;
            int pTo = fullText.LastIndexOf(")");
            return fullText[pFrom..pTo];
        }
    }
}
