using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Enums;
using Peter.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Repositories.UnitTests
{
    [TestFixture]
    public class CsvLineRegistryEntryWithIsin
    {
        private readonly CultureInfo cultureInfo = new CultureInfo("hu-HU");

        [TestCaseSource(nameof(TestCaseSource))]
        public void WithInvalidArray_ReturnsFalse(string[] inputStrings) =>
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(inputStrings, cultureInfo, out _)
            .Should().BeFalse();

        [Test]
        public void WithValidInput_ReturnsTrueAndValidResult()
        {
            var validLine = new string[]
            {
                "Aareal Bank AG",
                "DE0005408116",
                "http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116",
                "http://www.aareal-bank.com/investor-relations/",
                "2,08",
                "6",
                DateTime.Now.AddDays(1).Date.ToString(),
                string.Empty
            };

            var validResult = new RegistryEntry()
                {
                    Isin = "DE0005408116",
                    Name = "Aareal Bank AG",
                    OwnInvestorLink = "http://www.aareal-bank.com/investor-relations/",
                    StockExchangeLink = "http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116",
                    FinancialReport = new FinancialReport(2.08m, 6, DateTime.Now.AddDays(1).Date),
                    Position = Position.NoPosition
                };

            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(validLine, cultureInfo, out var result)
                .Should().BeTrue();
            result.Should().Be(validResult);
        }

        private static IEnumerable<IEnumerable<string>> TestCaseSource()
        {
            yield return new string[] { }; // empty
            yield return new string[] // too short
            {
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            };
            yield return new string[] // too long
            {
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            };
        }
    }
}
