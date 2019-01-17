using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;
using System;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class FinancialReport_TryParseNextReportDate
    {
        private DateTime _expectedResult = new DateTime(3000, 1, 1);

        [TestCase("3000.01.01")]
        public void ReturnsTrue_WhenInputIsValid(string input)
        {
            FinancialReport.TryParseNextReportDate(input, out var result).Should().BeTrue();
            result.Should().Be(_expectedResult);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("x")]
        public void ReturnsFalse_WhenInputIsInvalid(string input) =>
            FinancialReport.TryParseNextReportDate(input, out var _).Should().BeFalse();
    }
}
