using SearchEnginesApp.Services.SearchEngine;
using System.Collections.Generic;

namespace SearchEnginesApp.ViewModels
{
    public class SearchResultVM
    {
        public string EngineName { get; set; }
        public string Query { get; set; }
        public IEnumerable<FoundItemVM> Items { get; set; }
        public string Message { get; set; }
        public TrademarkLink TrademarkLink { get; set; }

        public void AddMessage(string message)
        {
            Message = string.IsNullOrEmpty(Message) ? message : $"{ Message } { message }";
        }
    }
}
