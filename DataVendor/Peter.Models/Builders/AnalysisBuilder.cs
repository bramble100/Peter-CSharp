using Peter.Models.Implementations;
using Peter.Models.Interfaces;

namespace Peter.Models.Builders
{
    public class AnalysisBuilder : IBuilder<IAnalysis>
    {
        private bool _nameIsSet;
        private bool _closingPriceSet;
        private bool _qtyInBuyingPacketIsSet;

        private string _name;
        private decimal _closingPrice;
        private int _qtyInBuyingPacket;
        private IFinancialAnalysis _financialAnalysis;
        private ITechnicalAnalysis _technicalAnalysis;

        public AnalysisBuilder SetName(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _name = value;
                _nameIsSet = true;
            }

            return this;
        }

        public AnalysisBuilder SetClosingPrice(decimal value)
        {
            if (value > 0)
            {
                _closingPrice = value;
                _closingPriceSet = true;
            }

            return this;
        }

        public AnalysisBuilder SetQtyInBuyingPacket(int value)
        {
            if (value > 0)
            {
                _qtyInBuyingPacket = value;
                _qtyInBuyingPacketIsSet = true;
            }

            return this;
        }

        public AnalysisBuilder SetFinancialAnalysis(IFinancialAnalysis value)
        {
            if (value != null)
            {
                _financialAnalysis = value;
            }

            return this;
        }

        public AnalysisBuilder SetTechnicalAnalysis(ITechnicalAnalysis value)
        {
            if (value != null)
            {
                _technicalAnalysis = value;
            }

            return this;
        }

        public IAnalysis Build()
        {
            if (!_nameIsSet || !_closingPriceSet || !_qtyInBuyingPacketIsSet)
            {
                return null;
            }

            return new Analysis()
            {
                Name = _name,
                ClosingPrice = _closingPrice,
                QtyInBuyingPacket = _qtyInBuyingPacket,
                FinancialAnalysis = _financialAnalysis,
                TechnicalAnalysis = _technicalAnalysis
            };
        }
    }
}