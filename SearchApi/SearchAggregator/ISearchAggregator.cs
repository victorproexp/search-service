using SearchApi.Models;
using System.Threading.Tasks;

namespace SearchApi
{
    public interface ISearchAggregator
    {
        Task<SearchResult> GetSearchResult(string query, int? maxAmount);
        Task<SearchResult> GetNormalizedSearchResult(string query, int? maxAmount);
    }
}
