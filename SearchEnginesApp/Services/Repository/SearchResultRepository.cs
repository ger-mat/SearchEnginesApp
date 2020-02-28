using Microsoft.EntityFrameworkCore;
using SearchEnginesApp.Models;
using SearchEnginesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEnginesApp.Services.Repository
{
    public class SearchResultRepository : ISearchResultRepository
    {
        readonly SearchContext db;

        public SearchResultRepository(SearchContext context)
        {
            db = context;
        }

        public async Task Add(SearchResultVM model)
        {
            var sr = new SearchResult
            {
                Query = model.Query,
                Date = DateTime.Now,
                EngineName = model.EngineName,
                Items =
                    model.Items
                    .Select(i => new FoundItem
                    {
                        Title = i.Title,
                        Snippet = i.Snippet,
                        Url = i.Url,
                    })
                    .ToList(),
            };

            db.SearchResults.Add(sr);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<DbSearchResultVM>> Contains(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await db.SearchResults
                .AsNoTracking()
                .Where(sr => sr.Query.Contains(query))
                .Select(sr => new DbSearchResultVM
                {
                    Date = sr.Date,
                    Engine = sr.EngineName,
                    Items = sr.Items.Select(i => new FoundItemVM
                    {
                        Title = i.Title,
                        Url = i.Url,
                        Snippet = i.Snippet,
                    }),
                })
                .ToListAsync();
        }

    }
}
