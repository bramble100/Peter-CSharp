using FluentAssertions;
using NUnit.Framework;

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
            string.Empty
        };

        [Test]
        public void WithEmptyArray_ReturnsFalse()
        {
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(_empty, out _).Should().BeFalse();
        }

        [Test]
        public void WithTooShortArray_ReturnsFalse()
        {
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(_tooShort, out _).Should().BeFalse();
        }
        [Test]
        public void WithTooLongArray_ReturnsFalse()
        {
            Peter.Repositories.Helpers.CsvLineRegistryEntryWithIsin.TryParseFromCsv(_tooLong, out _).Should().BeFalse();
        }
    }
}
