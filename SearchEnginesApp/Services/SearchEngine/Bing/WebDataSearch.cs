using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using Microsoft.Azure.CognitiveServices.Search.WebSearch.Models;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.SearchEngine.Bing
{
    public class WebDataSearch : IWebDataSearch
    {
        public async Task<SearchResponse> GetWebData(string query, string accessKey, int count)
        {
            using (var client = new WebSearchClient(new ApiKeyServiceClientCredentials(accessKey)))
            {
                return await client.Web.SearchAsync(query, count: count);
            }
        }
    }
}
