using Moq;
using SearchEnginesApp.Services.SearchEngine;
using SearchEnginesApp.Services.WebSearch;
using SearchEnginesApp.Tests.Services.SearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SearchEnginesApp.Tests.Services.WebSearch
{
    public class WebSearchServiceTests
    {
        private const int count = 10;
        private const string exceptionMessage = "Exception from Fault Engine";

        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public async Task Search_ArgumentNullException_Query(string query)
        {
            var service = new WebSearchService(Enumerable.Empty<ISearchEngine>());
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Search(query));
            Assert.Equal(nameof(query), ex.ParamName);
        }

        [Fact]
        public async Task Search_Over10EngineErrorMessage_OneEngine()
        {
            var engine = Over10SearchEngine();
            var service = new WebSearchService(
                new List<ISearchEngine> { engine });
            var model = await service.Search("query");
            Assert.Null(model.Items);
            Assert.Contains(WebSearchService.Over10EngineErrorMessage(engine.Name), model.Message);
        }

        [Fact]
        public async Task Search_Over10EngineErrorMessage_TwoEngines()
        {
            var engine = Over10SearchEngine();
            var service = new WebSearchService(
                new List<ISearchEngine> { engine, DelaySearchEngine(3000) });
            var model = await service.Search("query");
            Assert.Equal(count, model.Items.Count());
            Assert.Contains(WebSearchService.Over10EngineErrorMessage(engine.Name), model.Message);
        }

        [Fact]
        public async Task Search_EmptySearchEngineMessage_OneEngine()
        {
            var engine = EmptySearchEngine();
            var service = new WebSearchService(
                new List<ISearchEngine> { engine });
            var model = await service.Search("query");
            Assert.Null(model.Items);
            Assert.Contains(WebSearchService.NotFoundMessage(engine.Name), model.Message);
        }

        [Fact]
        public async Task Search_EmptySearchEngineMessage_TwoEngines()
        {
            var engine = EmptySearchEngine();
            var service = new WebSearchService(
                new List<ISearchEngine> { engine, DelaySearchEngine(3000) });
            var model = await service.Search("query");
            Assert.Equal(count, model.Items.Count());
            Assert.Contains(WebSearchService.NotFoundMessage(engine.Name), model.Message);
        }

        [Fact]
        public async Task Search_IsFaultedMessage_OneEngine()
        {
            var engine = FaultSearchEngine();
            var service = new WebSearchService(
                new List<ISearchEngine> { engine });
            var model = await service.Search("query");
            Assert.Null(model.Items);
            Assert.Contains(WebSearchService.IsFaultedMessage(
                engine.Name, exceptionMessage), model.Message);
        }

        [Fact]
        public async Task Search_IsFaultedMessage_TwoEngines()
        {
            var engine = FaultSearchEngine();
            var service = new WebSearchService(
                new List<ISearchEngine> { engine, DelaySearchEngine(3000) });
            var model = await service.Search("query");
            Assert.Equal(count, model.Items.Count());
            Assert.Contains(WebSearchService.IsFaultedMessage(
                engine.Name, exceptionMessage), model.Message);
        }

        [Fact]
        public async Task Search_HasOnlyFastestEngineFoundMessage_TwoEngines()
        {
            var workEngine = WorkSearchEngine();
            var slowEngine = DelaySearchEngine(3000);
            var service = new WebSearchService(
                new List<ISearchEngine> { slowEngine, workEngine });
            var model = await service.Search("query");
            Assert.Equal(count, model.Items.Count());
            Assert.Contains(WebSearchService.HasFoundMessage(
                workEngine.Name, count), model.Message);
            Assert.DoesNotContain(WebSearchService.HasFoundMessage(
                slowEngine.Name, count), model.Message);
        }

        private ISearchEngine FaultSearchEngine()
        {
            var mock = new Mock<ISearchEngine>();
            mock.Setup(se => se.Name).Returns("Fault Engine");
            mock.Setup(se => se.SearchFirst10("query"))
                .ThrowsAsync(new Exception(exceptionMessage));
            return mock.Object;
        }

        private ISearchEngine WorkSearchEngine()
        {
            var mock = new Mock<ISearchEngine>();
            mock.Setup(se => se.Name).Returns("Work Engine");
            mock.Setup(se => se.SearchFirst10("query"))
                .ReturnsAsync(FoundItemVMTestUtilities.GetTestFoundItems(count));
            return mock.Object;
        }

        private ISearchEngine DelaySearchEngine(int millisecondsTimeout)
        {
            var mock = new Mock<ISearchEngine>();
            mock.Setup(se => se.Name).Returns("Delay Engine");
            mock.Setup(se => se.SearchFirst10("query"))
                .Returns(FoundItemVMTestUtilities.GetTestFoundItemsWithDelayAsync(
                    count,millisecondsTimeout));
            return mock.Object;
        }

        private ISearchEngine Over10SearchEngine()
        {
            var mock = new Mock<ISearchEngine>();
            mock.Setup(se => se.Name).Returns("Over10 Engine");
            mock.Setup(se => se.SearchFirst10("query"))
                .ReturnsAsync(FoundItemVMTestUtilities.GetTestFoundItems(20));
            return mock.Object;
        }

        private ISearchEngine EmptySearchEngine()
        {
            var mock = new Mock<ISearchEngine>();
            mock.Setup(se => se.Name).Returns("Empty Engine");
            mock.Setup(se => se.SearchFirst10("query"))
                .ReturnsAsync(FoundItemVMTestUtilities.GetTestFoundItems(0));
            return mock.Object;
        }
    }
}
