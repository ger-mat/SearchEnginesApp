using Microsoft.Azure.CognitiveServices.Search.WebSearch.Models;
using Microsoft.Extensions.Options;
using Moq;
using SearchEnginesApp.Services.SearchEngine.Bing;
using SearchEnginesApp.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SearchEnginesApp.Tests.Services.SearchEngine.Bing
{
    public class BingSearchEngineTests
    {
        const int count = 10;

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_Query(string query)
        {
            var bingSearchEngine = new BingSearchEngine(
                GetOptions("accessKey"),
                GetMockWebDataSearchWith10Results());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                bingSearchEngine.SearchFirst10(query));
            Assert.Equal(nameof(query), ex.ParamName);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_AccessKey(string accessKey)
        {
            var bingSearchEngine = new BingSearchEngine(
                GetOptions(accessKey),
                GetMockWebDataSearchWith10Results());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                bingSearchEngine.SearchFirst10("query"));
            Assert.Equal(nameof(accessKey), ex.ParamName);
        }

        [Fact]
        public async Task Search_Success_With10Results()
        {
            var bingSearchEngine = new BingSearchEngine(
                GetOptions("accessKey"),
                GetMockWebDataSearchWith10Results());
            var result = await bingSearchEngine.SearchFirst10("query");
            Assert.Equal(FoundItemVMTestUtilities.GetTestFoundItems(count),
                result, new FoundItemVMComparer());
        }

        [Fact]
        public async Task Search_Success_WithEmptyResults()
        {
            var bingSearchEngine = new BingSearchEngine(
                GetOptions("accessKey"),
                GetMockWebDataSearchWithEmptyResults());
            var result = await bingSearchEngine.SearchFirst10("query");
            Assert.Empty(result);
        }

        private WebPage GetTestWebPage(int i)
        {
            return new WebPage(
                name: $"Title{ i }",
                url: $"Url{ i }",
                snippet: $"Snippet{ i }");
        }

        private SearchResponse GetTestSearchResponseWith10Results()
        {
            return new SearchResponse(
                webPages: new WebWebAnswer(value:
                    Enumerable.Range(0, count)
                    .Select(i => GetTestWebPage(i))
                    .ToList()));
        }

        private IWebDataSearch GetMockWebDataSearchWith10Results()
        {
            var mock = new Mock<IWebDataSearch>();
            mock.Setup(s => s.GetWebData("query", "accessKey", count))
                .ReturnsAsync(GetTestSearchResponseWith10Results());
            return mock.Object;
        }

        private IWebDataSearch GetMockWebDataSearchWithEmptyResults()
        {
            var mock = new Mock<IWebDataSearch>();
            mock.Setup(s => s.GetWebData("query", "accessKey", count))
                .ReturnsAsync(new SearchResponse());
            return mock.Object;
        }

        private IOptions<BingSearchOptions> GetOptions(string accessKey)
        {
            return Options.Create(new BingSearchOptions
            {
                AccessKey = accessKey
            });
        }
    }   
}
