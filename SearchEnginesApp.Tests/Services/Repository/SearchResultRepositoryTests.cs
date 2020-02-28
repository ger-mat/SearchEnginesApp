using Microsoft.EntityFrameworkCore;
using SearchEnginesApp.Models;
using SearchEnginesApp.Services.Repository;
using SearchEnginesApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SearchEnginesApp.Tests.Services.Repository
{
    public class SearchResultRepositoryTests
    {
        [Fact]
        public async Task Add_SuccessfulAdded()
        {
            var name = "Add_SuccessfulAdded";
            var options = GetTestContextOptions(name);
            var engineName = "TestEngine";
            using (var context = new SearchContext(options))
            {
                var service = new SearchResultRepository(context);
                await service.Add(GetTestSearchResutVM(name, "Query", engineName));
            }
            using (var context = new SearchContext(options))
            {
                Assert.Equal(1, context.SearchResults.Count());
                Assert.Equal($"{ name }+{ engineName }", context.SearchResults.Single().EngineName);
                Assert.Equal(2, context.FoundItems.Count());
                Assert.Equal(2, context.FoundItems.Where(i => i.Title.Contains("title")).Count());
            }
        }

        [Fact]
        public async Task Contains_Successful()
        {
            var excpected = 10;
            var options = GetTestContextOptions("Contains_Successful");
            using (var context = new SearchContext(options))
            {                
                var range = Enumerable
                        .Range(100, excpected)
                        .Select(i => GetTestSearchResut(i.ToString(),
                            "aBccDD", "TestEngine"));
                context.SearchResults.AddRange(range);
                context.SaveChanges();
            }
            using (var context = new SearchContext(options))
            {
                var service = new SearchResultRepository(context);
                var result = await service.Contains("BccD");
                Assert.Equal(excpected, result.Count());
            }
        }

        private SearchResultVM GetTestSearchResutVM(string preffix, string query, string engineName)
        {
            return new SearchResultVM
            {
                Query = $"{ preffix }_{ query }",
                EngineName = $"{ preffix }+{ engineName }",
                Items = new List<FoundItemVM>
                {
                    new FoundItemVM
                    {
                        Title = "title1",
                        Url = "url1",
                        Snippet = "snippet1",
                    },
                    new FoundItemVM
                    {
                        Title = "title1",
                        Url = "url1",
                        Snippet = "snippet1",
                    },
                }
            };
        }

        private SearchResult GetTestSearchResut(string preffix, string query, string engineName)
        {
            return new SearchResult
            {
                Query = $"{ preffix }_{ query }",
                EngineName = $"{ preffix }+{ engineName }",
                Items = new List<FoundItem>
                {
                    new FoundItem
                    {
                        Title = "title1",
                        Url = "url1",
                        Snippet = "snippet1",
                    },
                    new FoundItem
                    {
                        Title = "title1",
                        Url = "url1",
                        Snippet = "snippet1",
                    },
                }
            };
        }

        private DbContextOptions<SearchContext> GetTestContextOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<SearchContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }
    }
}
