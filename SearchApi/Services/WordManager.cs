namespace SearchApi.Services
{
    public class WordManager : IWordManager
    {
        private readonly Dictionary<string, int> mWords;

        public WordManager(Dictionary<string, int> words)
        {
            mWords = words;
        }

        public List<int> GetWordIds(string[] query, out List<string> outIgnored)
        {
            var res = new List<int>();
            var ignored = new List<string>();

            foreach (var aWord in query)
            {
                if (mWords.ContainsKey(aWord))
                {
                    res.Add(mWords[aWord]);
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
