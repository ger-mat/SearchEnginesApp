using System;
using System.Collections.Generic;

namespace SearchEnginesApp.ViewModels
{
    public class DbSearchResultVM
    {
        public string Engine { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<FoundItemVM> Items { get; set; }
    }
}
