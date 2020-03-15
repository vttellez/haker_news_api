using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HackerNews.Core.Proxy;
using HackerNews.Core.Proxy.Serialization;
using HackerNews.Services.NewsResponse;

namespace HackerNews.Services
{
    public class HackerNewsProxy : HttpProxy, IHackerNewsProxy
    {
        private HttpClient _httpClient;
        public HackerNewsProxy(HttpClient httpClient, IObjectSerializer objectSerializer) : base(httpClient, objectSerializer)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GetNewStoriesResponse>> GetNewStories()
        {
            List<GetNewStoriesResponse> respose = new List<GetNewStoriesResponse>();

            var url = $"{GetBaseAddress()}/newstories.json?print=pretty";

            IEnumerable<int> newStories = await GetAsync<IEnumerable<int>>(url);

            // For performance reasons I am just taking the first 20 stories
            // this is a candidate for pagination implementation

            //await AddNewsItem(respose, newStories.Take(20));

            await AddNewsItem(respose, newStories);
            return respose;
        }

        private async Task AddNewsItem(List<GetNewStoriesResponse> respose, IEnumerable<int> newStories)
        {
            if ((newStories == null && !newStories.Any()) || respose == null)
            {
                return;
            }

            foreach (int item in newStories)
            {
                respose.Add(await GetSotry(item));
            }
        }

        internal async Task<GetNewStoriesResponse> GetSotry(int id)
        {
            var url = $"{GetBaseAddress()}item/{id}.json?print=pretty";
            return await GetAsync<GetNewStoriesResponse>(url);
        }

        private string GetBaseAddress()
        {
            return _httpClient.BaseAddress.ToString();
        }
    }
}
