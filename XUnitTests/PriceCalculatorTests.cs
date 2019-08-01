using System;
using BusinessLayer;
using Xunit;

namespace BusinessTests
{
    public class PriceCalculatorTests
    {
        [Theory]
        [InlineData(1024.01, 890.44)]
        [InlineData(1024.019899, 890.45)]
        [InlineData(1024, 890.43)]
        [InlineData(0, 0)]
        public void GetTotalExcludingGstTest(decimal total, decimal expected)
        {
            var actual = PriceCalculator.GetTotalExcludingGst(total);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1024.01)]
        [InlineData(-1024)]
        public void GetTotalExcludingGstErrorTest(decimal total)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => PriceCalculator.GetTotalExcludingGst(total));
            Assert.Equal("Total is negative!\nParameter name: total", ex.Message);
        }
    }
}
