using FluentAssertions;
using Models.Builders;
using Models.Interfaces;
using NUnit.Framework;
using Repositories.Helpers;
using System;
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

            _validregistryEntry = new RegistryEntryBuilder()
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

            _validResult =
                $"\"Aareal Bank AG\"" +
                ";DE0005408116" +
                $@";http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116" +
                $@";http://www.aareal-bank.com/investor-relations/" +
                $";\"{2.08m.ToString(_cultureInfo)}\"" +
                ";6" +
                $";{DateTime.Now.AddDays(1).Date.ToString(_cultureInfo)}";
        }

        [Test]
        public void WithInvalidInput_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => CsvLineRegistryEntryWithIsin
                .FormatForCSV(null, ";", _cultureInfo));
            Assert.Throws<ArgumentNullException>(() => CsvLineRegistryEntryWithIsin
                .FormatForCSV(_validregistryEntry, null, _cultureInfo));
            Assert.Throws<ArgumentNullException>(() => CsvLineRegistryEntryWithIsin
                .FormatForCSV(_validregistryEntry, ";", null));
        }

        [Test]
        public void WithValidInput_ReturnsTrueAndValidResult()
        {
            var result = CsvLineRegistryEntryWithIsin
                .FormatForCSV(
                    _validregistryEntry,
                    ";",
                    _cultureInfo);

            result.Should().Be(_validResult);
        }
    }
}
