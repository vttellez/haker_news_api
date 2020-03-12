using System.Collections.Generic;
using System.Threading.Tasks;
using HackerNews.Services.NewsResponse;

namespace HackerNews.Services
{
    public interface IHackerNewsProxy
    {
        Task<IEnumerable<GetNewStoriesResponse>> GetNewStories();
    }
}
