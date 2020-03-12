using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNews.Core.Proxy
{
    public interface IHttpProxy
    {
        Task<TOut> GetAsync<TOut>(string uri);
        Task<TOut> PostAsync<TIn, TOut>(string uri, TIn model);
        Task<TOut> PutAsync<TIn, TOut>(string uri, TIn model);
        Task<TOut> DeleteAsync<TOut>(string uri);
        Task<TOut> DeleteAsync<TIn, TOut>(string uri, TIn model);
        Task<TOut> PatchAsync<TIn, TOut>(string uri, TIn model);
        Task<TOut> OptionsAsync<TIn, TOut>(string uri, TIn model);
        HttpClient HttpClient { get; }
    }
}
