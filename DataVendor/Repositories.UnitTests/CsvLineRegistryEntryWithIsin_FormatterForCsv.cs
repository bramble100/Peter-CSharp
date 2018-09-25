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
        private readonly KeyValuePair<string, IRegistryEntry> _validregistryEntry = new KeyValuePair<string, IRegistryEntry>(
            "DE0005408116",
            new RegistryEntry
            {
                Name = "Aareal Bank AG",
                OwnInvestorLink = "http://www.aareal-bank.com/investor-relations/",
                StockExchangeLink = "http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116",
                FinancialReport = new FinancialReport(2.08m, 6, DateTime.Now.AddDays(1).Date),
                Position = Position.NoPosition
            });
        private readonly string _validResult = 
            $@"Aareal Bank AG,DE0005408116,2.08,6,{DateTime.Now.AddDays(1).Date.ToString(new CultureInfo("us-EN"))},http://www.boerse-frankfurt.de/de/aktien/aareal+bank+ag+ag+DE0005408116,http://www.aareal-bank.com/investor-relations/,NoPosition";

        [Test]
        public void WithValidInput_ReturnsTrueAndValidResult() => 
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.FormatForCSV(_validregistryEntry, ",", new CultureInfo("us-EN")).Should().Be(_validResult);
    }
}
