using Microsoft.Azure.CognitiveServices.Search.WebSearch.Models;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.SearchEngine.Bing
{
    public interface IWebDataSearch
    {
        Task<SearchResponse> GetWebData(string query, string accessKey, int count);
    }
}
