using System.Collections.Generic;
using System.Threading.Tasks;
using HackerNews.Services.NewsResponse;

namespace HackerNews.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHackerNewsProxy _hackerNewsProxy;
        public HackerNewsService(IHackerNewsProxy hackerNewsProxy)
        {
            _hackerNewsProxy = hackerNewsProxy;
        }
        public async Task<IEnumerable<GetNewStoriesResponse>> GetNewStories()
        {
            return await _hackerNewsProxy.GetNewStories();
        }
    }
}
