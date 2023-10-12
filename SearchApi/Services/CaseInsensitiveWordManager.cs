namespace SearchApi.Services
{
    public class CaseInsensitiveWordManager : IWordManager
    {
        private readonly Dictionary<string, List<int>> mWords;

        public CaseInsensitiveWordManager(Dictionary<string, List<int>> words)
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
                    res.AddRange(mWords[aWord]);
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
