using System.Threading.Tasks;
using System.Xml.Linq;

namespace SearchEnginesApp.Services.SearchEngine.Yandex
{
    public interface IXDocumentLoader
    {
        Task<XDocument> Load(string query, string baseUrl, string user, string key);
    }
}