namespace SearchApi
{
    public class WordManager<T> : IWordManager
    {
        private readonly Dictionary<string, T> words;

        public WordManager(Dictionary<string, T> words)
        {
            this.words = words;
        }

        public List<int> GetWordIds(string[] query, out List<string> outIgnored)
        {
            var res = new List<int>();
            var ignored = new List<string>();

            foreach (var aWord in query)
            {
                if (words.ContainsKey(aWord))
                {
                    if (typeof(T) == typeof(int))
                    {
                        res.Add(Convert.ToInt32(words[aWord]));
                    }
                    else if (typeof(T) == typeof(List<int>))
                    {
                        if (words[aWord] is List<int> list)
                        {
                            res.AddRange(list);
                        }
                    }
                }
                else
                {
                    ignored.Add(aWord);
                }
            }

            outIgnored = ignored;
            return res;
        }
    }
}
