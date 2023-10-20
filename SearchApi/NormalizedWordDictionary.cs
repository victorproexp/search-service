namespace SearchApi
{
    public class NormalizedWordDictionary
    {
        public readonly Database database;

        public NormalizedWordDictionary()
        {
            var connectionString = ConnectionStringBuilder.Create("postgres2");

            database = new Database(connectionString);
        }

        public Dictionary<string, List<int>> GetAllWords()
        {
            var res = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

            // Create a parameterized SQL query with explicit casting to 'text'
            var sql = "SELECT \"id\", \"word\"::text FROM \"word\"";

            using (var cmd = database.CreateCommand(sql))
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var word = reader.GetString(1);

                    word = Database.ExtractWord(word);

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
    }
}
