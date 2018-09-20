using System;
using System.Globalization;

namespace Peter.Models.Validators
{
    public static class MarketDataEntity
    {
        public static bool TryParseClosingPrice(string input, out decimal output)
        {
            try
            {
                output = Convert.ToDecimal(input.Trim(), new CultureInfo("hu-HU"));
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
