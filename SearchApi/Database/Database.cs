using Npgsql;
using SearchApi.Models;

namespace SearchApi
{
    public class Database : IDatabase
    {
        private readonly NpgsqlConnection connection;
        private readonly string sqlGetAllWords = "SELECT \"id\", \"word\"::text FROM \"word\"";
        
        public Database()
        {
            var connectionString = ConnectionStringBuilder.Create();

            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public Database(string connectionString)
        {
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public List<KeyValuePair<int, int>> GetDocuments(List<int> wordIds)
        {
            var res = new List<KeyValuePair<int, int>>();

            // Create a parameterized SQL query
            var sql = "SELECT \"docId\", COUNT(\"wordId\") as count FROM \"occ\" WHERE \"wordId\" = ANY(@wordIds) GROUP BY \"docId\" ORDER BY COUNT(\"wordId\") DESC;";

            using (var cmd = CreateCommand(sql))
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

        public Dictionary<string, List<int>> GetAllWordsNormalized()
        {
            var res = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

            using (var cmd = CreateCommand(sqlGetAllWords))
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var word = ExtractWord(reader.GetString(1));

                    var normalizedWord = word.ToLower();

                    if (res.ContainsKey(normalizedWord))
                    {
                        // Handle duplicate words by adding the ID to the existing list
                        res[normalizedWord].Add(id);
                    }
                    else
                    {
                        // Add the word with a new list containing the ID
                        res.Add(normalizedWord, new List<int> { id });
                    }
                }
            }
            return res;
        }

        public Dictionary<string, int> GetAllWords()
        {
            var res = new Dictionary<string, int>();

            using (var cmd = CreateCommand(sqlGetAllWords))
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var word = ExtractWord(reader.GetString(1));

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

            using (var cmd = CreateCommand(sql))
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

        private static string ExtractWord(string fullText)
        {
            int pFrom = fullText.IndexOf(",") + ",".Length;
            int pTo = fullText.LastIndexOf(")");
            return fullText[pFrom..pTo];
        }

        private NpgsqlCommand CreateCommand(string sql)
        {
            return new NpgsqlCommand(sql, connection);
        }
    }
}
