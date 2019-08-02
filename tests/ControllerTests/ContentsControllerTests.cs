using System;
using AutoMapper;
using BusinessEntities;
using BusinessLayer;
using ContentExtractionService;
using ContentExtractionService.Controllers;
using ContentExtractionService.Models;
using ContentExtractService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ControllerTests
{
    public class ContentsControllerTests
    {
        [Fact]
        public void OkTest()
        {
            var mapper = new Mock<IMapper>();
            mapper.Setup(f => f.Map<Response>(It.IsAny<RelevantDataBO>()))
                .Returns(new Response() { Expense=new Expense() { Total=123.12M } });

            var extractor = new Mock<IContentExtractor>();
            extractor.Setup(f => f.GetRelevantData(It.IsAny<string>(), out It.Ref<RelevantDataBO>.IsAny))
                .Returns(true);

            var controller = new ContentsController(mapper.Object, extractor.Object);

            var request = new Request()
            {
                EmailContent = "<total>123.12</total>"
            };

            var result = controller.Extract(request);

            Assert.NotNull(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response>(okObjectResult.Value);
            Assert.Equal( 123.12M, response.Expense.Total);
        }

        [Fact]
        public void NoTotalTest()
        {
            var mapper = new Mock<IMapper>();
            mapper.Setup(f => f.Map<Response>(It.IsAny<RelevantDataBO>()))
                .Returns(new Response() { });

            var extractor = new Mock<IContentExtractor>();
            extractor.Setup(f => f.GetRelevantData(It.IsAny<string>(), out It.Ref<RelevantDataBO>.IsAny))
                .Returns(false);

            var controller = new ContentsController(mapper.Object, extractor.Object);

            var request = new Request()
            {
                EmailContent = "abc"
            };

            var result = controller.Extract(request);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void ExceptionTest1()
        {
            var mapper = new Mock<IMapper>();
            mapper.Setup(f => f.Map<Response>(It.IsAny<RelevantDataBO>()))
                .Returns(new Response() { });

            var extractor = new Mock<IContentExtractor>();
            extractor.Setup(f => f.GetRelevantData(It.IsAny<string>(), out It.Ref<RelevantDataBO>.IsAny))
                .Throws(new Exception());

            var controller = new ContentsController(mapper.Object, extractor.Object);

            var request = new Request();
           
            var result = controller.Extract(request);

            Assert.NotNull(result);
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.NotNull(statusCodeResult);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void ExceptionTest2()
        {
            var mapper = new Mock<IMapper>();
            mapper.Setup(f => f.Map<Response>(It.IsAny<RelevantDataBO>()))
                .Throws(new Exception());

            var extractor = new Mock<IContentExtractor>();
            extractor.Setup(f => f.GetRelevantData(It.IsAny<string>(), out It.Ref<RelevantDataBO>.IsAny))
                .Returns(true);

            var controller = new ContentsController(mapper.Object, extractor.Object);

            var request = new Request();

            var result = controller.Extract(request);

            Assert.NotNull(result);
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.NotNull(statusCodeResult);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
