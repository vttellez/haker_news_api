using System;
using HackerNews.Api.Controllers;
using HackerNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HackerNews.Tests
{
    [TestClass]
    public class NewsControllerTests
    {
        StoriesController _storiesController;
        private Mock<IHackerNewsService> _hackerNewsService;

        [TestInitialize]
        public void Initialize()
        {
            _hackerNewsService = new Mock<IHackerNewsService>();
            _storiesController = new StoriesController(_hackerNewsService.Object);
        }

        [TestMethod]
        public void StoriesController_Get_Should_Return_StatusCode_200()
        {
            //Arrange
            int extectedResult = 200;
            //Act
            var actualResult = (ObjectResult) _storiesController.Get().Result;
            //Assert
            Assert.AreEqual(extectedResult, actualResult.StatusCode);
        }

        [TestMethod]
        public void StoriesController_Get_Should_Return_StatusCode_500_When_NewsServieFail()
        {
            //Arrange
            _hackerNewsService.Setup(x => x.GetNewStories()).ReturnsAsync( () =>
            {
                throw new Exception("Unable to connect");
            });
            int extectedResult = 500;
            //Act
            var actualResult = (ObjectResult)_storiesController.Get().Result;
            //Assert
            Assert.AreEqual(extectedResult, actualResult.StatusCode);
        }
    }
}
