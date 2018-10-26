using System;
using System.Globalization;

namespace Peter.Models.Validators
{
    public static class MarketDataEntity
    {
        public static bool TryParseClosingPrice(string input, CultureInfo cultureInfo, out decimal output)
        {
            try
            {
                output = Convert.ToDecimal(input.Trim(), cultureInfo);
                return output > 0;
            }
            catch (Exception)
            {
                output = 0;
                return false;
            }
        }
    }
}
