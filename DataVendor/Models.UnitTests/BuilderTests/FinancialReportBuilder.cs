using FluentAssertions;
using NUnit.Framework;
using System;

namespace Models.UnitTests.BuilderTests
{
    [TestFixture]
    public class FinancialReportBuilder
    {
        [TestCase("4", "3", "2999.12.31", 4, 3)]
        public void ShouldReturnAnalysis_WhenInputValid(
            string eps, 
            string months, 
            string date,
            decimal expectedEPS,
            int expectedMonths)
        {
            var result = new Peter.Models.Builders.FinancialReportBuilder()
                .SetEPS(eps)
                .SetMonthsInReport(months)
                .SetNextReportDate(date)
                .Build();
            result.EPS.Should().Be(expectedEPS);
            result.MonthsInReport.Should().Be(expectedMonths);
            result.NextReportDate.Should().Be(new DateTime(2999, 12, 31));
        }

        [TestCase(null, "3", "2999.12.31")] // invalid eps
        [TestCase("", "3", "2999.12.31")] // invalid eps
        [TestCase("4", null, "2999.12.31")] // invalid months
        [TestCase("4", "", "2999.12.31")] // invalid months
        [TestCase("4", "0", "2999.12.31")] // invalid months
        [TestCase("4", "-2", "2999.12.31")] // invalid months
        [TestCase("4", "2", "2999.12.31")] // invalid months
        [TestCase("4", "3", null)] // invalid date
        [TestCase("4", "3", "")] // invalid date
        public void ShouldReturnNull_WhenInputInvalid(
            string eps,
            string months,
            string date)
        {
            var result = new Peter.Models.Builders.FinancialReportBuilder()
                .SetEPS(eps)
                .SetMonthsInReport(months)
                .SetNextReportDate(date)
                .Build();

            result.Should().BeNull();
        }
    }
}
