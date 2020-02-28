using Microsoft.Extensions.Options;
using Moq;
using SearchEnginesApp.Services.SearchEngine.Yandex;
using SearchEnginesApp.ViewModels;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace SearchEnginesApp.Tests.Services.SearchEngine.Bing
{
    public class YandexSearchEngineTests
    {
        const int count = 10;

        readonly static string yandexSamples = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase),
                "YandexSamples");

        readonly static string yandex_Error = 
            Path.Combine(yandexSamples, "Yandex_Error.xml");

        readonly static string yandex_OK =
           Path.Combine(yandexSamples, "Yandex_OK.xml");

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_Query(string query)
        {
            var yandexSearchEngine = new YandexSearchEngine(
                GetOptions("baseUrl", "user", "key"),
                GetMockXDocumentLoaderOK());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => yandexSearchEngine.SearchFirst10(query));
            Assert.Equal(nameof(query), ex.ParamName);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_BaseUrl(string baseUrl)
        {
            var yandexSearchEngine = new YandexSearchEngine(
                GetOptions(baseUrl, "user", "key"),
                GetMockXDocumentLoaderOK());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                yandexSearchEngine.SearchFirst10("query"));
            Assert.Equal(nameof(baseUrl), ex.ParamName);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_User(string user)
        {
            var yandexSearchEngine = new YandexSearchEngine(
                GetOptions("baseUrl", user, "key"),
                GetMockXDocumentLoaderOK());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                yandexSearchEngine.SearchFirst10("query"));
            Assert.Equal(nameof(user), ex.ParamName);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_Key(string key)
        {
            var yandexSearchEngine = new YandexSearchEngine(
                GetOptions("baseUrl", "user", key),
                GetMockXDocumentLoaderOK());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                yandexSearchEngine.SearchFirst10("query"));
            Assert.Equal(nameof(key), ex.ParamName);
        }

        [Fact]
        public async Task Search_Success_With10Results()
        {
            var yandexSearchEngine = new YandexSearchEngine(
                GetOptions("baseUrl", "user", "key"),
                GetMockXDocumentLoaderOK());
            var result = await yandexSearchEngine.SearchFirst10("query");
            Assert.Equal(
                FoundItemVMTestUtilities.GetTestFoundItemsWithHeadLineAndPassage(count),
                result, new FoundItemVMComparer());
        }

        [Fact]
        public async Task Search_With_Error()
        {
            var yandexSearchEngine = new YandexSearchEngine(
                GetOptions("baseUrl", "user", "key"),
                GetMockXDocumentLoaderWithError());
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                yandexSearchEngine.SearchFirst10("query"));
            Assert.Equal("TestError", ex.Message.Trim());
        }

        private IXDocumentLoader GetMockXDocumentLoaderOK()
        {
            var mock = new Mock<IXDocumentLoader>();
            mock.Setup(s => s.Load("query", "baseUrl", "user", "key"))
                .ReturnsAsync(XDocument.Load(yandex_OK));
            return mock.Object;
        }

        private IXDocumentLoader GetMockXDocumentLoaderWithError()
        {
            var mock = new Mock<IXDocumentLoader>();
            mock.Setup(s => s.Load("query", "baseUrl", "user", "key"))
                .ReturnsAsync(XDocument.Load(yandex_Error));
            return mock.Object;
        }

        private IOptions<YandexSearchOptions> GetOptions(string baseUrl, string user, string key)
        {
            return Options.Create(new YandexSearchOptions
            {
                BaseUrl = baseUrl,
                User = user,
                Key = key,
            });
        }
    }   
}
