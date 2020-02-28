using System;
using System.Collections.Generic;

namespace SearchEnginesApp.Models
{
    public class SearchResult
    {
        public SearchResult()
        {
            Items = new HashSet<FoundItem>();
        }
        public int Id { get; set; }
        public virtual ICollection<FoundItem> Items { get; set; }
        public string Query { get; set; }
        public string EngineName { get; set; }
        public DateTime Date { get; set; }
    }
}
