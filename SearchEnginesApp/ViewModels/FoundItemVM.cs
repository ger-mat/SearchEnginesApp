using System.Collections.Generic;

namespace SearchEnginesApp.ViewModels
{
    public class FoundItemVM
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Snippet { get; set; }
    }

    public class FoundItemVMComparer : IEqualityComparer<FoundItemVM>
    {
        public bool Equals(FoundItemVM x, FoundItemVM y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.Title == y.Title &&
                      x.Snippet == x.Snippet &&
                      x.Url == x.Url)
                return true;
            else return false;
        }

        public int GetHashCode(FoundItemVM obj)
        {
            int hCode = obj.Title.GetHashCode() ^ obj.Url.GetHashCode() ^ obj.Snippet.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}
