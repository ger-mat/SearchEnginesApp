using Microsoft.AspNetCore.Mvc;
using Moq;
using SearchEnginesApp.Controllers;
using SearchEnginesApp.Services.Repository;
using SearchEnginesApp.Services.WebSearch;
using SearchEnginesApp.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace SearchEnginesApp.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ViewResultWithDefaultQuery()
        {   
            HomeController controller = new HomeController(
                new Mock<IWebSearchService>().Object,
                new Mock<ISearchResultRepository>().Object);
            
            ViewResult result = controller.Index() as ViewResult;
            
            var model = Assert.IsAssignableFrom<SearchResultVM>(result.Model);
            Assert.Equal(HomeController.DefaultQuery, model.Query);
        }

        [Fact]
        public async Task DbSearch_ViewResult_IfNoQuery()
        {
            HomeController controller = new HomeController(
                new Mock<IWebSearchService>().Object,
                new Mock<ISearchResultRepository>().Object);

            ViewResult result = await controller.DbSearch() as ViewResult;

            var model = Assert.IsAssignableFrom<DbSearchVM>(result.Model);
            Assert.Null(model.Query);
            Assert.Null(model.SearchResults);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task WebSearch_WithNullOrWhiteSpaceQuery(string query)
        {
            HomeController controller = new HomeController(
                new Mock<IWebSearchService>().Object,
                new Mock<ISearchResultRepository>().Object);

            ViewResult result = await controller.WebSearch(query) as ViewResult;

            var model = Assert.IsAssignableFrom<SearchResultVM>(result.Model);
            Assert.Null(model.Query);
            Assert.Null(model.EngineName);
            Assert.Null(model.Items);
            Assert.Equal("Error: query is empty!", model.Message);
        }
    }
}
