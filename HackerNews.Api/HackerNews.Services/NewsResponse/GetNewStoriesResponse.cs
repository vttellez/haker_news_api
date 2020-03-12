using Newtonsoft.Json;

namespace HackerNews.Services.NewsResponse
{
    public class GetNewStoriesResponse
    {
        [JsonProperty("title")]
        public string Heading { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("by")]
        public string Author { get; set; }
    }
}
