namespace WebSearch
{
    public interface ISearchService
    {
        Task<SearchResult> GetSearchResultAsync(string query);
    }
}