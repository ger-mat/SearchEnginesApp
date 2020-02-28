using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.SearchEngine.Google
{
    public class CseSearch : ICseSearch
    {
        public async Task<Search> GetSearch(string query, string apiKey, string cx, long count)
        {
            using (var service = new CustomsearchService(
               new BaseClientService.Initializer { ApiKey = apiKey }))
            {
                var list = service.Cse.List(query);
                list.Cx = cx;
                list.Num = count;
                return await list.ExecuteAsync();
            }
        }
    }
}
