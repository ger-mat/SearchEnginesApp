using Google.Apis.Customsearch.v1.Data;
using Microsoft.Extensions.Options;
using Moq;
using SearchEnginesApp.Services.SearchEngine.Google;
using SearchEnginesApp.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SearchEnginesApp.Tests.Services.SearchEngine.Bing
{
    public class GoogleSearchEngineTests
    {
        const int count = 10;

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_Query(string query)
        {
            var googleSearchEngine = new GoogleSearchEngine(
                GetOptions("apiKey", "cx"),
                GetMockWebDataSearchWith10Results());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => googleSearchEngine.SearchFirst10(query));
            Assert.Equal(nameof(query), ex.ParamName);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_ApiKey(string apiKey)
        {
            var googleSearchEngine = new GoogleSearchEngine(
                GetOptions(apiKey, "cx"),
                GetMockWebDataSearchWith10Results());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                googleSearchEngine.SearchFirst10("query"));
            Assert.Equal(nameof(apiKey), ex.ParamName);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_Cx(string cx)
        {
            var googleSearchEngine = new GoogleSearchEngine(
                GetOptions("apiKey", cx),
                GetMockWebDataSearchWith10Results());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                googleSearchEngine.SearchFirst10("query"));
            Assert.Equal(nameof(cx), ex.ParamName);
        }

        [Fact]
        public async Task Search_Success_With10Results()
        {
            var googleSearchEngine = new GoogleSearchEngine(
                GetOptions("apiKey", "cx"),
                GetMockWebDataSearchWith10Results());
            var result = await googleSearchEngine.SearchFirst10("query");
            Assert.Equal(FoundItemVMTestUtilities.GetTestFoundItems(count),
                result, new FoundItemVMComparer());
        }

        [Fact]
        public async Task Search_Success_WithEmptyResults()
        {
            var googleSearchEngine = new GoogleSearchEngine(
                GetOptions("apiKey", "cx"),
                GetMockWebDataSearchWithEmptyResults());
            var result = await googleSearchEngine.SearchFirst10("query");
            Assert.Empty(result);
        }

        private Result GetResult(int i)
        {
            return new Result
            {
                Title = $"Title{ i }",
                Link = $"Url{ i }",
                Snippet = $"Snippet{ i }"
            };                
        }

        private Search GetTestSearchResponseWith10Results()
        {
            return new Search
            {
                Items = Enumerable.Range(0, count)
                    .Select(i => GetResult(i))
                    .ToList(),
            };
        }

        private ICseSearch GetMockWebDataSearchWith10Results()
        {
            var mock = new Mock<ICseSearch>();
            mock.Setup(s => s.GetSearch("query", "apiKey", "cx", count))
                .ReturnsAsync(GetTestSearchResponseWith10Results());
            return mock.Object;
        }

        private ICseSearch GetMockWebDataSearchWithEmptyResults()
        {
            var mock = new Mock<ICseSearch>();
            mock.Setup(s => s.GetSearch("query", "apiKey", "cx", count))
                .ReturnsAsync(new Search());
            return mock.Object;
        }

        private IOptions<GoogleSearchOptions> GetOptions(string apiKey, string cx)
        {
            return Options.Create(new GoogleSearchOptions
            {
                ApiKey = apiKey,
                Cx = cx
            });
        }        
    }   
}
