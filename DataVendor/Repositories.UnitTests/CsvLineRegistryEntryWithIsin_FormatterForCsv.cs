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
    public class CsvLineRegistryEntryWithIsin_FormatterForCsv
    {
        private readonly IRegistryEntry _validregistryEntry;
        private readonly CultureInfo _cultureInfo;
        private readonly string _validResult;

        public CsvLineRegistryEntryWithIsin_FormatterForCsv()
        {
            _cultureInfo = CultureInfo.CurrentCulture;

            _validregistryEntry = new RegistryEntry
            {
                Isin = "DE0005408116",
                Name = "Aareal Bank AG",
                OwnInvestorLink = "http://www.aareal-bank.com/investor-relations/",
                StockExchangeLink = "http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116",
                FinancialReport = new FinancialReport(2.08m, 6, DateTime.Now.AddDays(1).Date),
                Position = Position.NoPosition
            };

            _validResult =
                $"\"Aareal Bank AG\"" +
                ";DE0005408116" +
                $@";http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116" +
                $@";http://www.aareal-bank.com/investor-relations/" +
                $";\"{2.08m.ToString(_cultureInfo)}\"" +
                ";6" +
                $";{DateTime.Now.AddDays(1).Date.ToString(_cultureInfo)}" +
                $";NoPosition";
        }

        [Test]
        public void WithValidInput_ReturnsTrueAndValidResult()
        {
            var result = Peter
                .Repositories
                .Helpers
                .CsvLineRegistryEntryWithIsin
                .FormatForCSV(
                    _validregistryEntry,
                    ";",
                    _cultureInfo);

            result.Should().Be(_validResult);
        }
    }
}
