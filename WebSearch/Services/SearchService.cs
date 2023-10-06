namespace WebSearch
{
    public class SearchService : ISearchService
    {
        static readonly HttpClient client = new();
        
        static readonly string HOST_URL = "https://localhost:7232/search?query=";

        public async Task<SearchResult> GetSearchResultAsync(string query)
        {
            SearchResult? searchResult = null;
            HttpResponseMessage response = await client.GetAsync(HOST_URL + query);
            if (response.IsSuccessStatusCode)
            {
                searchResult = await response.Content.ReadAsAsync<SearchResult>();
            }
            return searchResult!;
        }
    }
}
