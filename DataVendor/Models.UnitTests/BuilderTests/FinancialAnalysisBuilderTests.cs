using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Builders;

namespace Models.UnitTests.BuilderTests
{
    [TestFixture]
    public class FundamentalAnalysisBuilderTests
    {
        [TestCase(24, 4, 3, 1.5)]
        public void ShouldReturnValidObject_WhenValidInput(
            decimal price, 
            decimal eps, 
            int months, 
            decimal expectedPE)
        {
            var result = new FundamentalAnalysisBuilder()
                .SetClosingPrice(price)
                .SetEPS(eps)
                .SetMonthsInReport(months)
                .Build();
            result.PE.Should().Be(expectedPE);
        }

        [TestCase(0, 4, 3)] // invalid price
        [TestCase(-1, 4, 3)] // invalid price
        [TestCase(24, 0, 3)] // zero eps
        [TestCase(24, 4, 0)] // invalid months
        [TestCase(24, 4, 2)] // invalid months
        [TestCase(24, null, 2)] // no eps
        [TestCase(24, 4, null)] // no months
        public void ShouldReturnNull_WhenInputInvalid(
            decimal price,
            decimal eps,
            int months)
        {
            var result = new Peter.Models.Builders.FundamentalAnalysisBuilder()
                .SetClosingPrice(price)
                .SetEPS(eps)
                .SetMonthsInReport(months)
                .Build();

            result.Should().BeNull();
        }
    }
}
