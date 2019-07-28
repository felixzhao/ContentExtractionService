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
            Assert.Equal(response.Expense.Total, 123.12M);
        }
    }
}
