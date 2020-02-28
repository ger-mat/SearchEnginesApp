using Microsoft.Extensions.Options;
using SearchEnginesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.SearchEngine.Google
{
    public class GoogleSearchEngine : ISearchEngine
    {
        const long count = 10;
        readonly string apiKey;
        readonly string cx;
        readonly ICseSearch cseSearch;
        public string Name => "Google";
        public TrademarkLink TrademarkLink =>
            new TrademarkLink("https://cse.google.com", "google.png", "enchanced by ", " Google Custom Search");

        public GoogleSearchEngine(IOptions<GoogleSearchOptions> options, ICseSearch cseSearch)
        {
            apiKey = options.Value.ApiKey;
            cx = options.Value.Cx;
            this.cseSearch = cseSearch;
        }

        public async Task<IEnumerable<FoundItemVM>> SearchFirst10(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }
            if (string.IsNullOrWhiteSpace(cx))
            {
                throw new ArgumentNullException(nameof(cx));
            }
            var items = new List<FoundItemVM>();
            var webData = await cseSearch.GetSearch(query, apiKey, cx, count);
            if (webData?.Items?.Count > 0)
            {
                items.AddRange(webData.Items.Select(r => new FoundItemVM
                {
                    Title = r.Title,
                    Snippet = r.Snippet,
                    Url = r.Link,
                }));
            }
            return items;
        }
    }
}
