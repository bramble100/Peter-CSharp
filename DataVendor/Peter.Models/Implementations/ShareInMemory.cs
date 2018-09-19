using System;
using System.Collections.Generic;
using System.Linq;

namespace Peter_Registry.Models
{
    public class ShareInMemory
    {
        public const int MAX_LENGTH_OF_NAME = 50;
        public const int MIN_LENGTH_OF_NAME = 2;
        public const int LENGTH_OF_ISIN = 12;

        private readonly HashSet<string> _validPositions = new HashSet<string>()
        {
            string.Empty,
            "long",
            "short"
        };
        private string _name;
        private string _isin;
        private int? _monthsInReport;
        private DateTime? _nextReportDate;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="shareStrings">String array.</param>
        /// <exception cref="ArgumentException">When either the name or the ISIN is missing.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When either the name or the ISIN is missing.</exception>
        /// <exception cref="ArgumentNullException">When either the name or the ISIN is missing.</exception>
        public ShareInMemory(IEnumerable<string> shareStrings)
        {
            ValidateInput(shareStrings, out string[] shareStringsArray);

            Name = shareStringsArray[0];
            ISIN = shareStringsArray[1];

            if (decimal.TryParse(shareStringsArray[2], out decimal resultDec))
            {
                EPS = resultDec;
            }

            if (int.TryParse(shareStringsArray[3], out int resultInt))
            {
                MonthsInReport = resultInt;
            }

            if (DateTime.TryParse(shareStringsArray[4], out DateTime resultDate))
            {
                NextReportDate = resultDate;
            }

            if (Uri.TryCreate(shareStringsArray[5], UriKind.Absolute, out Uri resultUri))
            {
                StockExchangeLink = resultUri;
            }

            if (Uri.TryCreate(shareStringsArray[6], UriKind.Absolute, out resultUri))
            {
                OwnInvestorLink = resultUri;
            }

            if (_validPositions.Contains(shareStringsArray[7].ToLower()))
            {
                Position = shareStringsArray[7].ToLower();
            }
        }

        private static void ValidateInput(IEnumerable<string> shareStrings, out string[] shareStringsArray)
        {
            if (shareStrings == null)
            {
                throw new ArgumentNullException(nameof(shareStrings));
            }
            else if (!shareStrings.Any())
            {
                throw new ArgumentException("Argument cannot be empty.", nameof(shareStrings));
            }

            shareStringsArray = shareStrings.ToArray();

            if (shareStrings.Count() < 8)
            {
                throw new ArgumentException("Argument must contain at least 8 fields.", nameof(shareStrings));
            }
            else if (string.IsNullOrEmpty(shareStringsArray[0]))
            {
                throw new ArgumentException("ISIN cannot be null or empty for creating a share.");
            }
            else if (string.IsNullOrEmpty(shareStringsArray[1]))
            {
                throw new ArgumentException("Name cannot be null or empty for creating a share.");
            }

            // TODO: adjust validation here and in property setters
        }

        /// <summary>
        /// Name of the share.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value), "Name can not be null or empty.");
                }
                else if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Name can not be null or whitespace.");
                }
                value = value.Trim();
                if (value.Length > MAX_LENGTH_OF_NAME)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"Name should be not longer than {MAX_LENGTH_OF_NAME} character.");
                }
                else if (value.Length < MIN_LENGTH_OF_NAME)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"Name should be not shorter than {MIN_LENGTH_OF_NAME} character.");
                }

                _name = value;
            }
        }
        /// <summary>
        /// ISIN (International Securities Identification Number) of the share.
        /// </summary>
        public string ISIN
        {
            get => _isin;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "ISIN can not be null, empty or whitespace.");
                }

                value = value.Trim();
                if (value.Any(c => !char.IsLetterOrDigit(c)))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"ISIN should contain only letters or digits.");
                }
                if (!value.Length.Equals(LENGTH_OF_ISIN))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"ISIN should be {LENGTH_OF_ISIN} character long.");
                }

                _isin = value;
            }
        }
        /// <summary>
        /// Fast Simple Moving Average (calculated from the market data).
        /// </summary>
        public decimal? FastSMA { get; set; }
        /// <summary>
        /// Slow Simple Moving Average (calculated from the market data).
        /// </summary>
        public decimal? SlowSMA { get; set; }
        /// <summary>
        /// Price/Earning ratio.
        /// </summary>
        public decimal? PE { get; set; }
        /// <summary>
        /// Earning Per Share.
        /// </summary>
        public decimal? EPS { get; set; }
        /// <summary>
        /// How many months are included in the financial report (EPS).
        /// </summary>
        public int? MonthsInReport
        {
            get => _monthsInReport;
            set
            {
                if (value is null)
                {
                    _nextReportDate = null;
                }
                else
                {
                    if (!new HashSet<int>() { 3, 6, 9, 12 }.Contains((int)value))
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), $"Number {value} is not a valid number of months.");
                    }
                }
                _monthsInReport = value;
            }
        }
        /// <summary>
        /// The date on which the next financial report will be published.
        /// </summary>
        public DateTime? NextReportDate
        {
            get => _nextReportDate;
            set
            {
                if (value is null)
                {
                    _monthsInReport = null;
                }
                _nextReportDate = value;
            }
        }
        public Uri OwnInvestorLink { get; set; }
        public Uri StockExchangeLink { get; set; }
        /// <summary>
        /// Valid values are "long", "short", or can be left empty.
        /// </summary>
        public string Position { get; set; }

        public decimal? ClosingPrice { get; set; }
        public bool IsBuyable { get; set; } = false;
        public bool IsUpdateable => ReportIsOutDatedOrMissing
            || EPS == null
            || MonthsInReport == null
            || NextReportDate == null
            || StockExchangeLink == null;

        public bool ReportIsOutDatedOrMissing =>
            (NextReportDate == null) || NextReportDate < DateTime.Now.Date;
    }
}
