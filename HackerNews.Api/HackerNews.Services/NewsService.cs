using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HackerNews.Core.Caching;
using HackerNews.Services.NewsResponse;

[assembly: InternalsVisibleTo("HackerNews.Tests")]
namespace HackerNews.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHackerNewsProxy _hackerNewsProxy;
        private readonly IStaticCacheManager _staticCacheManager;
        public HackerNewsService(
            IHackerNewsProxy hackerNewsProxy,
            IStaticCacheManager staticCacheManager
        )
        {
            _hackerNewsProxy = hackerNewsProxy;
            _staticCacheManager = staticCacheManager;
        }
        public async Task<IEnumerable<GetNewStoriesResponse>> GetNewStories(short page, short count)
        {

            string key = "All_Newest_stories";

            IEnumerable<GetNewStoriesResponse> newStories = await GetStoriesAsync(key);

            return newStories?.Skip(Skip(page, count))?.Take(count);
        }

        public virtual async Task<IEnumerable<GetNewStoriesResponse>> GetStoriesAsync(string key)
        {
            return await _staticCacheManager.Get(key, async () =>
            {
                var result = new List<GetNewStoriesResponse>();
                await AddItemsAsync(result);
                return result;
            });
        }

        private async Task AddItemsAsync(List<GetNewStoriesResponse> result)
        {
            async Task<IEnumerable<GetNewStoriesResponse>> LoadStoriesFuncAsync() => await GetStoriesAsync();

            foreach (var item in await LoadStoriesFuncAsync())
            {
                result.Add(item);
            }
        }

        public virtual async Task<IEnumerable<GetNewStoriesResponse>> GetStoriesAsync()
        {
            return await _hackerNewsProxy.GetNewStories();
        }

        internal int Skip(short page, short count)
        {
            return count * (page - 1);
        }
    }
}
