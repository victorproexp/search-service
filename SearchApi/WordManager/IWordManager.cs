namespace SearchApi
{
    public interface IWordManager
    {
        List<int> GetWordIds(string[] query, out List<string> outIgnored);
    }
}
