namespace HackerNews.Core.Proxy.Validation
{
    public class ValidationResultError
    {
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
