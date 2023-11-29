using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SearchApi.Models;

namespace SearchApi
{
    public class SearchCache : ISearchCache
    {
        private readonly MemoryCache cache = new(Options.Create(new MemoryCacheOptions()));

        public SearchResult? GetCachedResult(string cacheKey)
        {
            if (cache.TryGetValue(cacheKey, out SearchResult? result))
            {
                Console.WriteLine($"Cache hit: {cacheKey}");
                return result;
            }

            return null;
        }

        public void CacheResult(string cacheKey, SearchResult searchResult)
        {
            cache.Set(cacheKey, searchResult, TimeSpan.FromMinutes(5));
            Console.WriteLine($"Cache set: {cacheKey}");
        }
    }
}
