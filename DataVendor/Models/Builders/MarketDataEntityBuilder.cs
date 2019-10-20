using Models.Implementations;
using Models.Interfaces;
using System;

namespace Models.Builders
{
    public class MarketDataEntityBuilder : IBuilder<IMarketDataEntity>
    {
        private bool _closingPriceSet;
        private bool _dateTimeSet;
        private bool _nameSet;

        private decimal _closingPrice;
        private DateTime _dateTime;
        private string _isin;
        private string _name;
        private decimal _previousDayclosingPrice;
        private string _stockExchange;
        private int _volumen;

        public MarketDataEntityBuilder SetClosingPrice(decimal value)
        {
            if (value > 0)
            {
                _closingPrice = value;
                _closingPriceSet = true;
            }

            return this;
        }

        public MarketDataEntityBuilder SetDateTime(DateTime value)
        {
            _dateTime = value;
            _dateTimeSet = true;
            return this;
        }

        public MarketDataEntityBuilder SetIsin(string value)
        {
            _isin = value;
            return this;
        }

        public MarketDataEntityBuilder SetName(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _name = value;
                _nameSet = true;
            }
            return this;
        }

        public MarketDataEntityBuilder SetPreviousDayClosingPrice(decimal value)
        {
            _previousDayclosingPrice = value;

            return this;
        }

        public MarketDataEntityBuilder SetStockExchange(string value)
        {
            _stockExchange = value;
            return this;
        }

        public MarketDataEntityBuilder SetVolumen(int value)
        {
            if (value > 0)
            {
                _volumen = value;
            }
            return this;
        }

        public IMarketDataEntity Build()
        {
            return (_closingPriceSet && _dateTimeSet && _nameSet) 
                ? new MarketDataEntity()
                {
                    ClosingPrice = _closingPrice,
                    DateTime = _dateTime,
                    Isin = _isin,
                    Name = _name,
                    PreviousDayClosingPrice = _previousDayclosingPrice,
                    StockExchange = _stockExchange ?? string.Empty,
                    Volumen = _volumen
                } 
                : null;
        }
    }
}
