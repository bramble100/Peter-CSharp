using System;
using System.Collections.Generic;

namespace Peter.Models.Validators
{
    public static class FinancialReport
    {
        public static bool TryParseEPS(string input, out decimal output)
        {
            output = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;

            try
            {
                output = Convert.ToDecimal(input);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryParseMonthsInReport(string input, out int output)
        {
            output = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;

            try
            {
                output = Convert.ToInt32(input);
                return new HashSet<int>() { 3, 6, 9, 12 }.Contains(output);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryParseNextReportDate(string input, out DateTime output)
        {
            output = DateTime.Now;
            if (string.IsNullOrWhiteSpace(input)) return false;

            try
            {
                output = Convert.ToDateTime(input);
                return output > DateTime.Now;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
