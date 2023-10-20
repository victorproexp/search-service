namespace SearchApi
{
    public class NormalizedWordManager : IWordManager
    {
        private readonly Dictionary<string, List<int>> words;

        public NormalizedWordManager(Dictionary<string, List<int>> words)
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
                    res.AddRange(words[aWord]);
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
