using Microsoft.Extensions.Options;
using SearchEnginesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SearchEnginesApp.Services.SearchEngine.Yandex
{
    public class YandexSearchEngine : ISearchEngine
    {
        public const string passageSeparator = " | ";
        readonly string baseUrl;
        readonly string user;
        readonly string key;
        readonly IXDocumentLoader xDocumentLoader;
        public string Name => "Yandex";
        public TrademarkLink TrademarkLink =>
            new TrademarkLink("https://yandex.com", "yandex.png", null, " has found answers");

        public YandexSearchEngine(IOptions<YandexSearchOptions> options, IXDocumentLoader xDocumentLoader)
        {
            baseUrl = options.Value.BaseUrl;
            user = options.Value.User;
            key = options.Value.Key;
            this.xDocumentLoader = xDocumentLoader;
        }

        public async Task<IEnumerable<FoundItemVM>> SearchFirst10(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var xdoc = await xDocumentLoader.Load(query, baseUrl, user, key);
            var error = xdoc
                .Descendants("error")
                .Select(e => e.Value)
                .FirstOrDefault();
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }
            else
            {
                return xdoc
                .Descendants("doc")
                .Select(doc => new FoundItemVM
                {
                    Title = doc.Element("title").Value,
                    Url = doc.Element("url").Value,
                    Snippet = GetSnippet(doc),
                });
            }
        }

        private string GetSnippet(XElement doc)
        {
            var headline = doc.Descendants("headline").Select(h => h.Value);
            var passages = doc.Descendants("passage").Select(h => h.Value);
            var all = headline.Any() ? headline.Union(passages) : passages;
            return string.Join(passageSeparator, all);
        }
    }
}
