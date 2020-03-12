using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using HackerNews.Api;
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

        private HackerNewsService _hackerNewsService;
        private Mock<IHackerNewsProxy> _hackerNewsProxy;
        private Fixture _fixture;

        [TestInitialize]
        public void Initialize()
        {
            _hackerNewsProxy = new Mock<IHackerNewsProxy>();
            _hackerNewsService = new HackerNewsService(_hackerNewsProxy.Object);
            _fixture = new Fixture();
           // string[] args = new string[] { "% LAUNCHER_ARGS %" };
           //var f=  CreateHostBuilder(args);
           // f.Build().Services()
           
        }
        [TestMethod]
        public void HackerNewsService_GetNewStories_Sould_Return_ListOf_GetNewStoriesResponse()
        {
            //Arrange
            int expectedNumberOfItems = 10;

            _hackerNewsProxy.Setup(x => x.GetNewStories()).Returns(async () =>
            {
                IEnumerable<GetNewStoriesResponse> t = _fixture.CreateMany<GetNewStoriesResponse>(10);
                return await Task.FromResult(t);
            });
            //Act
            IEnumerable<GetNewStoriesResponse> actualResult = _hackerNewsService.GetNewStories().Result;
            //Assert
            Assert.AreEqual(expectedNumberOfItems, actualResult.Count());
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
        // private void Methodd()
        // {
        //     var hostBuilder = new HostBuilder()
        //.ConfigureWebHost(webHost =>
        //{
        //// Add TestServer
        //webHost.Build();

        //// Specify the environment
        //webHost.UseEnvironment("Test");

        //    webHost.Configure(app => app.Run();
        //});
        // }

    }



}
