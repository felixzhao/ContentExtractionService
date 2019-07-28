using System;
using BusinessEntities;
using BusinessLayer;
using Xunit;

namespace XUnitTests
{
    public class RejectTests
    {
        [Theory]
        [InlineData(@"abc")]
        [InlineData(@"abc<total>123efg")]
        [InlineData(@"abc<abc@gmail.com>123efg")]
        public void SuccessTest(string content)
        {
            IContentExtractor extractor = new ContentExtractor();
            RelevantDataBO actual;
            var result = extractor.GetRelevantData(content, out actual);

            Assert.False(result);
        }
    }
}
