using Models.Enums;
using Models.Implementations;
using Models.Interfaces;
using System;

namespace Models.Builders
{
    public class RegistryEntryBuilder : IBuilder<IRegistryEntry>
    {
        private bool _isinIsSet;
        private bool _nameIsSet;

        private string _isin;
        private Uri _stockExchangeLink;
        private Uri _ownInvestorLink;
        private string _name;
        private IFundamentalAnalysis _fundamentalAnalysis;
        private IFinancialReport _financialReport;

        public RegistryEntryBuilder SetIsin(string value)
        {
            if (Validators.Isin.IsValid(value))
            {
                _isin = value;
                _isinIsSet = true;
            }

            return this;
        }

        public RegistryEntryBuilder SetName(string value)
        {
            if (Validators.RegistryEntry.TryParseName(value, out var name))
            {
                _name = name;
                _nameIsSet = true;
            }

            return this;
        }

        public RegistryEntryBuilder SetOwnInvestorLink(string value)
        {
            if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                _ownInvestorLink = new Uri(value);

            return this;
        }

        public RegistryEntryBuilder SetStockExchangeLink(string value)
        {
            if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                _stockExchangeLink = new Uri(value);

            return this;
        }

        public RegistryEntryBuilder SetFundamentalAnalysis(IFundamentalAnalysis fundamentalAnalysis)
        {
            _fundamentalAnalysis = fundamentalAnalysis;

            return this;
        }

        public RegistryEntryBuilder SetFinancialReport(IFinancialReport financialReport)
        {
            _financialReport = financialReport;

            return this;
        }

        public IRegistryEntry Build() => (_isinIsSet && _nameIsSet)
                ? new RegistryEntry()
                {
                    Isin = _isin,
                    Name = _name,
                    OwnInvestorLink = _ownInvestorLink,
                    StockExchangeLink = _stockExchangeLink,
                    FundamentalAnalysis = _fundamentalAnalysis,
                    FinancialReport = _financialReport
                }
                : null;
    }
}