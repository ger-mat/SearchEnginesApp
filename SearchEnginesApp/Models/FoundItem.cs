namespace SearchEnginesApp.Models
{
    public class FoundItem
    {
        public int Id { get; set; }
        public int SearchResultId { get; set; }
        public virtual SearchResult SearchResult { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }
        public string Snippet { get; set; }
    }
}
