using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SearchApi.Models;

namespace SearchApi
{
    public class SearchCache
    {
        private readonly MemoryCache cache = new(Options.Create(new MemoryCacheOptions()));

        public static string CreateCacheKey(SearchParameters parameters)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(parameters.IsNormalized ? "N_" : "NN_");
            stringBuilder.Append(parameters.Query);
            stringBuilder.Append(':');
            stringBuilder.Append(parameters.MaxAmount);

            Console.WriteLine(stringBuilder);

            return stringBuilder.ToString();
        }

        public SearchResult? GetCachedResult(string cacheKey)
        {
            if (cache.TryGetValue(cacheKey, out SearchResult? result))
            {
                return result;
            }

            return null;
        }

        public void CacheResult(string cacheKey, SearchResult searchResult)
        {
            cache.Set(cacheKey, searchResult, TimeSpan.FromMinutes(5));
        }
    }
}