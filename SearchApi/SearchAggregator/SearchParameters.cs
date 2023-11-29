namespace SearchApi
{
    public class SearchParameters
    {
        private const int DefaultMaxAmount = 10;
        public string Query { get; }
        public int MaxAmount { get; }

        public SearchParameters(string query, int? maxAmount = null)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            MaxAmount = maxAmount ?? DefaultMaxAmount;
        }
    }
}
