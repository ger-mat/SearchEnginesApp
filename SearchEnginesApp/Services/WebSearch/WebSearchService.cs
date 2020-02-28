using SearchEnginesApp.Services.SearchEngine;
using SearchEnginesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.WebSearch
{
    public class WebSearchService : IWebSearchService
    {
        readonly IList<ISearchEngine> searchEngines;
        public WebSearchService(IEnumerable<ISearchEngine> searchEngines)
        {
            this.searchEngines = searchEngines.ToList();
        }

        public static string Over10EngineErrorMessage(string engineName) =>
            $"{ engineName } engine error: has no limit (10).";
        public static string NotFoundMessage(string engineName) =>
            $"{ engineName } did not find anything.";
        public static string HasFoundMessage(string engineName, int count) =>
            $"{ engineName } has found { count } answers:";
        public static string IsFaultedMessage(string engineName, string exceptionMessage) =>
            $"{ engineName } is faulted: { exceptionMessage }";

        public async Task<SearchResultVM> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            var model = new SearchResultVM { Query = query };

            var tasks = searchEngines
            .Select(se => se.SearchFirst10(query))
            .ToList();
            while (tasks.Count > 0)
            {
                var fastestTask = await Task.WhenAny(tasks);
                var searchEngine = searchEngines[tasks.IndexOf(fastestTask)];
                var engineName = searchEngine.Name;
                if (fastestTask.IsCompletedSuccessfully)
                {
                    var count = fastestTask.Result.Count();
                    if (count > 0 && count <= 10)
                    {
                        model.EngineName = engineName;
                        model.Items = fastestTask.Result;
                        model.TrademarkLink = searchEngine.TrademarkLink;
                        model.AddMessage(HasFoundMessage(engineName, count));
                        break;
                    }
                    else if (count > 10)
                    {
                        model.AddMessage(Over10EngineErrorMessage(engineName));
                        tasks.Remove(fastestTask);
                    }
                    else // 0
                    {
                        model.AddMessage(NotFoundMessage(engineName));
                        tasks.Remove(fastestTask);
                    }
                }
                else //only IsFault (cancel not implemented)
                {
                    var message = IsFaultedMessage(engineName, fastestTask.Exception.InnerException.Message);
                    model.AddMessage(message);
                    tasks.Remove(fastestTask);
                }
            }

            return model;
        }
    }
}
