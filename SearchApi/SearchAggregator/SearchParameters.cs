namespace SearchApi
{
    public class SearchParameters
    {
        private const int DefaultMaxAmount = 10;
        public bool IsNormalized { get; }
        public string Query { get; }
        public int MaxAmount { get; }
        
        public SearchParameters(bool isNormalized, string query, int? maxAmount = null)
        {
            IsNormalized = isNormalized;
            Query = query ?? throw new ArgumentNullException(nameof(query));
            MaxAmount = maxAmount ?? DefaultMaxAmount; 
        }
    }
}
