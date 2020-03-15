using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HackerNews.Api;
using HackerNews.Services.NewsResponse;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xunit;

namespace HackerNews.Tests
{
    public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    {
    }

    [TestClass]
    public class NewStoriesIntegration : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private HttpClient _client;


        [TestInitialize]
        public void Initialize()
        {
            _client = new TestingWebAppFactory<Startup>().CreateClient();
        }


        [TestMethod]
        public async Task Get()
        {
            //For performance issues only return one story
            var responseJson = await _client.GetStringAsync("stories/1/1");

            IEnumerable<GetNewStoriesResponse> actualResponse = JsonConvert.DeserializeObject<List<GetNewStoriesResponse>>(responseJson);

            Assert.IsTrue(actualResponse.Count() > 0);
        }
    }
}
