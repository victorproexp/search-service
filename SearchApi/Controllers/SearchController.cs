using Microsoft.AspNetCore.Mvc;
using SearchApi.Models;

namespace SearchApi.Controllers;

[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchAggregator searchAggregator;

    public SearchController(ISearchAggregator searchAggregator)
    {
        this.searchAggregator = searchAggregator;
    }

    [HttpGet("search")]
    public async Task<SearchResult> Search(string query, int? maxAmount) =>
        await searchAggregator.GetSearchResult(query, maxAmount);

    [HttpGet("nsearch")]
    public async Task<SearchResult> NormalizedSearch(string query, int? maxAmount) =>
        await searchAggregator.GetNormalizedSearchResult(query, maxAmount);
}
