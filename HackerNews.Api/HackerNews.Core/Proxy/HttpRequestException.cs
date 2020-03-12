using System;
using System.Net;

namespace HackerNews.Core.Proxy
{
    public class HttpRequestException : Exception
    {
        public string ContentAsString;
        public Uri RequestUri;
        public HttpStatusCode StatusCode;

        public HttpRequestException(string message)
            : base(message)
        {
        }

        public HttpRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
