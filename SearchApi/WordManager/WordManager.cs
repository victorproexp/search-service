namespace SearchApi
{
    public class WordManager : IWordManager
    {
        private readonly Dictionary<string, int> words;

        public WordManager(Dictionary<string, int> words)
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
                    res.Add(words[aWord]);
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
