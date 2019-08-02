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

        [Theory]
        [InlineData(1024.01, 890.44, 133.57)]
        [InlineData(1024.019899, 890.45, 133.57)]
        [InlineData(1024, 890.43, 133.57)]
        [InlineData(0, 0, 0)]
        public void GetGstTest(decimal total, decimal totalExcludingGst, decimal expected)
        {
            var actual = PriceCalculator.GetGST(total, totalExcludingGst);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1024.01, 890.44)]
        [InlineData(-1024, 890.43)]
        public void GetGstErrorTest(decimal total,decimal totalExcludingGst)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => PriceCalculator.GetGST(total, totalExcludingGst));
            Assert.Equal("Total is negative!\nParameter name: total", ex.Message);
        }
    }
}
