using NUnit.Framework;
using ContentExtractionService.Controllers;
using ContentExtractionService.Models;
using ContentExtractService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContentExtractionService.Tests
{
    public class RejectTest
    {
        //[SetUp]
        //public void Setup()
        //{

        //}

        //private Response GetTestResponse(string content)
        //{
            //ContentsController home = new ContentsController();
            //Request request = new Request() { EmailContent = content };
            //var actionResult = home.Extract2(request);
            ////Assert
            //var okObjectResult = actionResult as OkObjectResult;
            //Assert.NotNull(okObjectResult);
            //return okObjectResult.Value as Response;
        //}

        [Test]
        public void NotTotal()
        {
            string content = @"abc";
           
            Response expected = new Response() { StatusCode = 0, StatusDescription= "reject" };

            //var actual = GetTestResponse(content);
            //Assert.NotNull(actual);
            //Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            //Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
        }

        [Test]
        public void NonClosingTag()
        {
            //string content = @"abc<total>123efg";

            //Response expected = new Response() { StatusCode = 0, StatusDescription = "reject" };

            //var actual = GetTestResponse(content);
            //Assert.NotNull(actual);
            //Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            //Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
        }

        [Test]
        public void ContentWrongFormat()
        {
            //string content = @"abc<abc@gmail.com>123efg";

            //Response expected = new Response() { StatusCode = 0, StatusDescription = "reject" };

            //var actual = GetTestResponse(content);
            //Assert.NotNull(actual);
            //Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            //Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
        }
    }
}
