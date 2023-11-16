namespace SearchApi
{
    public class WordManager<T> : IWordManager
    {
        private readonly Dictionary<string, T> words;

        public WordManager(Dictionary<string, T> words)
        {
            this.words = words;
        }

        public List<int> GetWordIds(string[] query, out List<string> outignoredWords)
        {
            var result = new List<int>();
            var ignoredWords = new List<string>();

            if (typeof(T) == typeof(int))
            {
                foreach (var word in query)
                {
                    if (words.ContainsKey(word))
                    {
                        result.Add(Convert.ToInt32(words[word]));
                    }
                    else
                    {
                        ignoredWords.Add(word);
                    }
                }
            }
            else if (typeof(T) == typeof(List<int>))
            {
                foreach (var word in query)
                {
                    if (words.ContainsKey(word) && words[word] is List<int> list)
                    {
                        result.AddRange(list);
                    }
                    else
                    {
                        ignoredWords.Add(word);
                    }
                }
            }

            outignoredWords = ignoredWords;
            return result;
        }
    }
}
