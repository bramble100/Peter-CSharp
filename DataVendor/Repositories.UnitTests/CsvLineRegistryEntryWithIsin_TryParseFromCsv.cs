using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Builders;
using Peter.Models.Enums;
using Peter.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Repositories.UnitTests
{
    [TestFixture]
    public class CsvLineRegistryEntryWithIsin_TryParseFromCsv
    {
        private static readonly CultureInfo _cultureInfo = new CultureInfo("hu-HU");
        private static readonly string[] _validLine = new string[]
            {
                "Aareal Bank AG",
                "DE0005408116",
                "http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116",
                "http://www.aareal-bank.com/investor-relations/",
                "2,08",
                "6",
                DateTime.Now.AddDays(1).Date.ToString()
            };

        [TestCaseSource(nameof(TestCaseSource))]
        public void WithInvalidArray_ReturnsFalse(string[] inputStrings) =>
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(inputStrings, _cultureInfo, out _)
            .Should().BeFalse();

        [Test]
        public void WithValidInput_ReturnsTrueAndValidResult()
        {
            var validResult = new RegistryEntryBuilder()
                .SetIsin("DE0005408116")
                .SetName("Aareal Bank AG")
                .SetOwnInvestorLink("http://www.aareal-bank.com/investor-relations/")
                .SetStockExchangeLink("http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116")
                .SetFinancialReport(new FinancialReportBuilder()
                    .SetEPS(2.08m)
                    .SetMonthsInReport(6)
                    .SetNextReportDate(DateTime.Now.AddDays(1).Date)
                    .Build())
                .Build();

            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(_validLine, _cultureInfo, out var result)
                .Should().BeTrue();
            result.Should().Be(validResult);
        }

        private static IEnumerable<IEnumerable<string>> TestCaseSource()
        {
            string[] invalidLine = new string[8];

            yield return new string[] { }; // empty
            yield return new string[] // too short
            {
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
            };
            yield return new string[] // no content at all
            {
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            };

            _validLine.CopyTo(invalidLine, 0);
            invalidLine[0] = string.Empty;
            yield return invalidLine; // no name

            _validLine.CopyTo(invalidLine, 0);
            invalidLine[1] = string.Empty;
            yield return invalidLine; // no ISIN
        }
    }
}
