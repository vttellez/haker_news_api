using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using HackerNews.Api;
using HackerNews.Core.Caching;
using HackerNews.Services;
using HackerNews.Services.NewsResponse;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HackerNews.Tests
{
    [TestClass]
    public class NewsServiceTests
    {
        private Mock<HackerNewsService> _mockHackerNewsService;
        private HackerNewsService _hackerNewsService;
        private Mock<IHackerNewsProxy> _hackerNewsProxy;
        private Mock<IStaticCacheManager> _staticCacheManager;
        private Fixture _fixture;

        [TestInitialize]
        public void Initialize()
        {
            _hackerNewsProxy = new Mock<IHackerNewsProxy>();
            _staticCacheManager = new Mock<IStaticCacheManager>();
            _hackerNewsService = new HackerNewsService(_hackerNewsProxy.Object, _staticCacheManager.Object);
            _mockHackerNewsService = new Mock<HackerNewsService>(_hackerNewsProxy.Object, _staticCacheManager.Object);
            _fixture = new Fixture();
        }
        [TestMethod]
        public async Task HackerNewsService_GetNewStories_Sould_Return_ListOf_GetNewStoriesResponseAsync()
        {
            //Arrange
            int expectedNumberOfItems = 10;
            _mockHackerNewsService.Setup(x => x.GetStoriesAsync(It.IsAny<string>())).Returns(async () =>
            {
                IEnumerable<GetNewStoriesResponse> t = _fixture.CreateMany<GetNewStoriesResponse>(10);
                return await Task.FromResult(t.ToList());
            });

            //Act
            IEnumerable<GetNewStoriesResponse> actualResult = await _mockHackerNewsService.Object.GetNewStories(1, 10);
            //Assert
            Assert.AreEqual(expectedNumberOfItems, actualResult.Count());
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               }
       );

        [TestMethod]
        public void HackerNewsService_Skip_Should_Skip_10_When_Page_Is_1()
        {
            //Arrange
            int expectedSkipCount = 0;
            short page = 1;
            short count = 10;
            //Act
            var actualResult = _hackerNewsService.Skip(page, count);
            //Assert
            Assert.AreEqual(expectedSkipCount, actualResult);
        }
    }



}
