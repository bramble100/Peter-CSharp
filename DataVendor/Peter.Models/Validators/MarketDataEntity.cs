using System;
using System.Globalization;

namespace Peter.Models.Validators
{
    public static class MarketDataEntity
    {
        public static bool TryParseClosingPrice(string input, CultureInfo cultureInfo, out decimal output)
        {
            output = 0;
            if (string.IsNullOrWhiteSpace(input) || cultureInfo is null) return false;

            try
            {
                output = Convert.ToDecimal(input, cultureInfo);
                return output > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
