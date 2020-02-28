using Microsoft.AspNetCore.Mvc;
using SearchEnginesApp.Services.Repository;
using SearchEnginesApp.Services.WebSearch;
using SearchEnginesApp.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEnginesApp.Controllers
{
    public class HomeController : Controller
    {
        public const string DefaultQuery = "Write your query";
        readonly IWebSearchService webSearchService;
        readonly ISearchResultRepository repository;

        public HomeController(IWebSearchService webSearchService, ISearchResultRepository repository)
        {
            this.webSearchService = webSearchService;
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new SearchResultVM { Query = DefaultQuery });
        }

        [HttpPost]
        public async Task<IActionResult> WebSearch(string query)
        {
            var model = new SearchResultVM { Query = query };
            if (string.IsNullOrWhiteSpace(query))
            {
                model.Query = null;
                model.AddMessage("Error: query is empty!");
            }
            else
            {
                model = await webSearchService.Search(query);
                if (model.Items?.Any() == true)
                {
                    await repository.Add(model);
                }
            }

            return View("Index", model);
        }


        [HttpGet]
        public async Task<IActionResult> DbSearch(string query = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new DbSearchVM());
            }
            else
            {
                query = query.Trim();
                return View(new DbSearchVM
                {
                    Query = query,
                    SearchResults = await repository.Contains(query),
                });
            }
        }
    }
}