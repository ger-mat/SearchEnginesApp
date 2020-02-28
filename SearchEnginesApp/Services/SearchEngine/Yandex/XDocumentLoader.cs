using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SearchEnginesApp.Services.SearchEngine.Yandex
{
    public class XDocumentLoader : IXDocumentLoader
    {
        public async Task<XDocument> Load(string query, string baseUrl, string user, string key)
        {
            var url = $"{ baseUrl }&user={ user }&key={ key }&query={ query }";
            var request = WebRequest.Create(url);
            using (var response = await request.GetResponseAsync())
            {
                using (var xmlReader = XmlReader.Create(response.GetResponseStream()))
                {
                    return XDocument.Load(xmlReader);
                }
            }
        }
    }
}