using Google.Apis.Customsearch.v1.Data;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.SearchEngine.Google
{
    public interface ICseSearch
    {
        Task<Search> GetSearch(string query, string apiKey, string cx, long count);
    }
}
