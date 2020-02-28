using SearchEnginesApp.ViewModels;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.WebSearch
{
    public interface IWebSearchService
    {
        Task<SearchResultVM> Search(string query);
    }
}