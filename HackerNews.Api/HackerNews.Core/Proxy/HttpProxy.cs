using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HackerNews.Core.Proxy.Serialization;

namespace HackerNews.Core.Proxy
{
    /// <summary>
    /// Handles Http requests
    /// </summary>
    public class HttpProxy : IHttpProxy
    {
        private readonly IObjectSerializer _objectSerializer;

        public HttpClient HttpClient { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="objectSerializer"></param>
        public HttpProxy(HttpClient httpClient, IObjectSerializer objectSerializer)
        {
            HttpClient = httpClient;
            _objectSerializer = objectSerializer;
        }

        /// <summary>
        /// Http request Get async 
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<TOut> GetAsync<TOut>(string uri)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await MakeHttpRequestAsync<TOut>(requestMessage);
            }
        }

        /// <summary>
        /// Http request Post async
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="uri"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TOut> PostAsync<TIn, TOut>(string uri, TIn model)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                return await PrepareAndMakeHttpCallWithContentAsync<TIn, TOut>(requestMessage, model);
            }
        }

        /// <summary>
        /// Http request Put async
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="uri"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TOut> PutAsync<TIn, TOut>(string uri, TIn model)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri))
            {
                return await PrepareAndMakeHttpCallWithContentAsync<TIn, TOut>(requestMessage, model);
            }
        }

        /// <summary>
        /// Http request Delete async
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<TOut> DeleteAsync<TOut>(string uri)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri))
            {
                return await MakeHttpRequestAsync<TOut>(requestMessage);
            }
        }

        /// <summary>
        /// Http request Delete async
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="uri"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TOut> DeleteAsync<TIn, TOut>(string uri, TIn model)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri))
            {
                return await PrepareAndMakeHttpCallWithContentAsync<TIn, TOut>(requestMessage, model);
            }
        }


        /// <summary>
        /// Http request Patch async
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="uri"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TOut> PatchAsync<TIn, TOut>(string uri, TIn model)
        {
            using (var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), uri))
            {
                return await PrepareAndMakeHttpCallWithContentAsync<TIn, TOut>(requestMessage, model);
            }
        }

        /// <summary>
        /// Http request options
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="uri"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TOut> OptionsAsync<TIn, TOut>(string uri, TIn model)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Options, uri))
            {
                return await PrepareAndMakeHttpCallWithContentAsync<TIn, TOut>(requestMessage, model);
            }
        }

        /// <summary>
        /// Invokes http request
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual async Task<TOut> MakeHttpRequestAsync<TOut>(HttpRequestMessage message)
        {
            var httpClient = HttpClient;


            var requestUri = message.RequestUri;
            using (var httpResponse = await httpClient.SendAsync(message))
            {
                var content = await CheckResponseForErrorsAndExtractContentAsync(httpResponse, requestUri);

                return _objectSerializer.Deserialize<TOut>(content);
            }
        }


        /// <summary>
        /// Prepare http request
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="httpRequestMessage"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual async Task<TOut> PrepareAndMakeHttpCallWithContentAsync<TIn, TOut>(HttpRequestMessage httpRequestMessage, TIn model)
        {
            AddHttpRequestMethodContentTypeAttributes(httpRequestMessage, model);
            return await MakeHttpRequestAsync<TOut>(httpRequestMessage);
        }


        /// <summary>
        /// Adds http request content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequestMessage"></param>
        /// <param name="model"></param>
        protected virtual void AddHttpRequestMethodContentTypeAttributes<T>(HttpRequestMessage httpRequestMessage, T model)
        {
            var serializedMessage = _objectSerializer.Serialize(model);
            httpRequestMessage.Content = new StringContent(serializedMessage,
                                                           _objectSerializer.Encoding,
                                                           _objectSerializer.ContentType);
        }

        /// <summary>
        ///     This method offers an opportunity for child classes derived from <see cref="HttpProxy" />
        ///     to handle the response message.
        /// </summary>
        /// <param name="httpResponse">The response from the endpoint located at <see cref="requestUri" /></param>
        /// <param name="requestUri">The URI of the endpoint that returned the <see cref="httpResponse" /></param>
        /// <returns>
        ///     A boolean indicating if the response was handled or not.
        ///     If the response was handled in the override then it can return true to prevent the usual handling
        /// </returns>
        protected virtual async Task<bool> HandleResponse(HttpResponseMessage httpResponse, Uri requestUri)
        {
            return await Task.Run(() => false);
        }


        /// <summary>
        /// Handles response errors
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        private async Task<string> CheckResponseForErrorsAndExtractContentAsync(HttpResponseMessage httpResponse, Uri requestUri)
        {
            var statusCode = httpResponse.StatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                var method = httpResponse.RequestMessage.Method.Method;
                httpResponse.Dispose();
                throw new UnauthorizedAccessException($"Uri: {requestUri}, Method: {method}");
            }

            string content = string.Empty;

            if (httpResponse.Content != null)
            {
                content = await httpResponse.Content.ReadAsStringAsync();
            }

            if (await HandleResponse(httpResponse,
                                          requestUri))
            {
                httpResponse.Dispose();
                return content;
            }

            httpResponse.Dispose();

            if (statusCode != HttpStatusCode.OK &&
                statusCode != HttpStatusCode.NoContent)
            {
                throw new HttpRequestException(content ?? "Unhandled HttpRequestException")
                {
                    StatusCode = statusCode,
                    RequestUri = requestUri,
                    ContentAsString = content
                };
            }

            return content;
        }

    }
}
