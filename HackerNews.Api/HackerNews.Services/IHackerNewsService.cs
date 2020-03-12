using System.Collections.Generic;
using System.Threading.Tasks;
using HackerNews.Services.NewsResponse;

namespace HackerNews.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<GetNewStoriesResponse>> GetHackerNews();
    }
}
