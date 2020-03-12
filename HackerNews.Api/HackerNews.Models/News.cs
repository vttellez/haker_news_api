using System;

namespace HackerNews.Models
{
    public class News
    {
        public Guid ExternalKey { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
