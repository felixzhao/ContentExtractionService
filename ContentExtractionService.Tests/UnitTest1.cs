using NUnit.Framework;
using ContentExtractionService.Controllers;
using ContentExtractionService.Models;
using ContentExtractService.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using log4net;
using log4net.Config;

namespace Tests
{
    public class Tests
    {
        

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            

            ContentsController home = new ContentsController();
            Request request = new Request() { EmailContent = "abc" };
            
            Response expected = new Response() { StatusCode = 0 };

            var actionResult = home.Extract2(request);
            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var actual = okObjectResult.Value as Response;
            Assert.NotNull(actual);

    

            Assert.AreEqual(expected.StatusCode, actual.StatusCode);

            //Assert.Pass();
        }
    }
}