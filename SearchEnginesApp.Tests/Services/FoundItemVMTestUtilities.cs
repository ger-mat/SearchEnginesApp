using SearchEnginesApp.Services.SearchEngine.Yandex;
using SearchEnginesApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEnginesApp.Tests.Services
{
    static class FoundItemVMTestUtilities
    {
        public static IEnumerable<FoundItemVM> GetTestFoundItemsWithHeadLineAndPassage(int count)
        {
            return Enumerable.Range(0, count)
                .Select(i => GetTestFoundItemWithHeadLineAndPassage(i))
                .ToList();
        }

        public static IEnumerable<FoundItemVM> GetTestFoundItems(int count)
        {
            return Enumerable.Range(0, count)
                .Select(i => GetTestFoundItem(i))
                .ToList();
        }

        public static async Task<IEnumerable<FoundItemVM>> GetTestFoundItemsWithDelayAsync(
            int count, int millisecondsTimeout)
        {
            await Task.Delay(millisecondsTimeout);
            return GetTestFoundItems(count);
        }

        private static FoundItemVM GetTestFoundItem(int i)
        {
            return new FoundItemVM
            {
                Title = $"Title{ i }",
                Url = $"Url{ i }",
                Snippet = $"Snippet{ i }"
            };
        }

        private static FoundItemVM GetTestFoundItemWithHeadLineAndPassage(int i)
        {
            return new FoundItemVM
            {
                Title = $"Title{ i }",
                Url = $"Url{ i }",
                Snippet = 
                    $"Headline{ i }{ YandexSearchEngine.passageSeparator }Passage{ i }",
            };
        }
    }
}
