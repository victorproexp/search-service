using Shared;
using Npgsql;

namespace SearchApi.Database
{
    public class CaseInsensitivePostgresDatabase
    {
        public readonly PostgresDatabase postgresDatabase; // Composition

        public CaseInsensitivePostgresDatabase()
        {
            postgresDatabase = new PostgresDatabase();
        }

        public Dictionary<string, List<int>> GetAllWordsCI()
        {
            var res = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

            // Create a parameterized SQL query with explicit casting to 'text' and ILIKE for case-insensitive comparison
            var sql = "SELECT \"id\", \"word\"::text FROM \"word\"";

            using (var cmd = postgresDatabase.CreateCommand(sql))
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var w = reader.GetString(1);

                    // Extract word
                    int pFrom = w.IndexOf(",") + ",".Length;
                    int pTo = w.LastIndexOf(")");
                    w = w[pFrom..pTo];

                    // Normalize the word to lowercase
                    var normalizedWord = w.ToLower();

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
    }
}