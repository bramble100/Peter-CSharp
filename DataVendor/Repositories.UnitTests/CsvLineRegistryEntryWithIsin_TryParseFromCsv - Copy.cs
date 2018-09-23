using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Enums;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Repositories.UnitTests
{
    [TestFixture]
    public class CsvLineRegistryEntryWithIsin
    {
        private readonly string[] _empty = new string[] { };
        private readonly string[] _tooShort = new string[]
        {
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        };
        private readonly string[] _tooLong = new string[]
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
        private readonly string[] _validLine = new string[]
        {
            "Aareal Bank AG",
            "DE0005408116",
            "2.08",
            "6",
            DateTime.Now.AddDays(1).Date.ToString(),
            "http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116",
            "http://www.aareal-bank.com/investor-relations/",
            string.Empty
        };

        private readonly KeyValuePair<string, IRegistryEntry> _validResult = new KeyValuePair<string, IRegistryEntry>(
            "DE0005408116",
            new RegistryEntry
            {
                Name = "Aareal Bank AG",
                OwnInvestorLink = new Uri("http://www.aareal-bank.com/investor-relations/"),
                StockExchangeLink = new Uri("http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116"),
                FinancialReport = new FinancialReport(2.08m, 6, DateTime.Now.AddDays(1).Date),
                Position = Position.NoPosition
            });

        [Test]
        public void WithEmptyArray_ReturnsFalse() =>
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(_empty, out _)
            .Should().BeFalse();

        [Test]
        public void WithTooShortArray_ReturnsFalse() =>
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(_tooShort, out _)
            .Should().BeFalse();

        [Test]
        public void WithTooLongArray_ReturnsFalse() =>
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(_tooLong, out _)
            .Should().BeFalse();

        [Test]
        public void WithValidInput_ReturnsTrueAndValidResult()
        {
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(_validLine, out var result).Should().BeTrue();
            result.Should().Be(_validResult);
        }
    }
}
