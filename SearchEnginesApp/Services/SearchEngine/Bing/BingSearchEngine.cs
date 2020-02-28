using Microsoft.Extensions.Options;
using SearchEnginesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.SearchEngine.Bing
{
    public class BingSearchEngine : ISearchEngine
    {
        const int count = 10;
        readonly string accessKey;
        readonly IWebDataSearch webDataSearch;
        public string Name => "Bing";
        public TrademarkLink TrademarkLink =>
            new TrademarkLink("https://bing.com", "bing.png", "powered by ", null);

        public BingSearchEngine(IOptions<BingSearchOptions> options, IWebDataSearch webDataSearch)
        {
            accessKey = options.Value.AccessKey;
            this.webDataSearch = webDataSearch;
        }

        public async Task<IEnumerable<FoundItemVM>> SearchFirst10(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (string.IsNullOrWhiteSpace(accessKey))
            {
                throw new ArgumentNullException(nameof(accessKey));
            }
            var items = new List<FoundItemVM>();
            var webData = await webDataSearch.GetWebData(query, accessKey, count);
            if (webData?.WebPages?.Value?.Count > 0)
            {
                items.AddRange(webData.WebPages.Value.Select(p => new FoundItemVM
                {
                    Title = p.Name,
                    Snippet = p.Snippet,
                    Url = p.Url,
                }));
            }
            return items;
        }
    }
}
