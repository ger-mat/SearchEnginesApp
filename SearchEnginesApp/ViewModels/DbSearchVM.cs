using System.Collections.Generic;

namespace SearchEnginesApp.ViewModels
{
    public class DbSearchVM : ISearchVM
    {
        public string Query { get; set; }
        public IEnumerable<DbSearchResultVM> SearchResults { get; set; }
    }
}
