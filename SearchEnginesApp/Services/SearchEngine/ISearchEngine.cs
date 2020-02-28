using SearchEnginesApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.SearchEngine
{
    public interface ISearchEngine
    {
        string Name { get; }
        TrademarkLink TrademarkLink { get; }
        Task<IEnumerable<FoundItemVM>> SearchFirst10(string query);
    }
}
