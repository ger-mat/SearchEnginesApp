namespace SearchEnginesApp.Services.SearchEngine
{
    public sealed class TrademarkLink
    {
        public TrademarkLink(string hyperlink, string image, string preffixMessage, string suffixMessage)
        {
            Hyperlink = hyperlink;
            Image = image;
            PreffixMessage = preffixMessage;
            SuffixMessage = suffixMessage;
        }
        public string Hyperlink { get; private set; }
        public string Image { get; private set; }
        public string PreffixMessage { get; private set; }
        public string SuffixMessage { get; private set; }
    }
}
