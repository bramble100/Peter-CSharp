using FluentAssertions;
using Models.Builders;
using Models.Interfaces;
using NUnit.Framework;

namespace Models.UnitTests.BuilderTests
{
    [TestFixture]
    public class AnalysisBuilderTests
    {
        [TestCase(4.9, "Ok", 100)]
        public void ShouldReturnValidObject_WhenValidInput(decimal price, string name, int qty)
        {
            ITechnicalAnalysis technicalAnalysis = new TechnicalAnalysisBuilder()
                .SetFastSMA(1)
                .SetSlowSMA(2)
                .Build();

            IAnalysis result = new AnalysisBuilder()
                .SetClosingPrice(price)
                .SetName(name)
                .SetQtyInBuyingPacket(qty)
                .SetTechnicalAnalysis(technicalAnalysis)
                .Build();

            result.Should().NotBeNull();
            result.ClosingPrice.Should().Be(price);
            result.Name.Should().Be(name);
            result.QtyInBuyingPacket.Should().Be(qty);
            result.TechnicalAnalysis.Should().Be(technicalAnalysis);
        }

        [TestCase(-3, "Ok", 100)] // negative price
        [TestCase(0, "Ok", 100)] // zero price
        [TestCase(4, null, 100)] // no name
        [TestCase(4, "", 100)] // no name
        [TestCase(4, "\t", 100)] // whitespace name
        [TestCase(4, "Ok", -7)] // negative qty
        [TestCase(4, "Ok", 0)] // zero qty
        public void ShouldReturnNull_WhenInvalidInput(decimal price, string name, int qty)
        {
            IAnalysis result = new AnalysisBuilder()
                .SetClosingPrice(price)
                .SetName(name)
                .SetQtyInBuyingPacket(qty)
                .SetTechnicalAnalysis(new TechnicalAnalysisBuilder()
                    .SetFastSMA(1)
                    .SetSlowSMA(2)
                    .Build())
                .Build();

            result.Should().BeNull();
        }
    }
}
