using SearchApi.Models;

namespace SearchApi
{
    public interface ISearchCache
    {
        SearchResult? GetCachedResult(string cacheKey);
        void CacheResult(string cacheKey, SearchResult searchResult);
    }
}