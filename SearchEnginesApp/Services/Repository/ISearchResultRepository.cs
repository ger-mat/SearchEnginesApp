using SearchEnginesApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.Repository
{
    public interface ISearchResultRepository
    {
        Task Add(SearchResultVM model);
        Task<IEnumerable<DbSearchResultVM>> Contains(string query);
    }
}