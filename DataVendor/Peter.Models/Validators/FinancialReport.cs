using System;
using System.Collections.Generic;

namespace Peter.Models.Validators
{
    public static class FinancialReport
    {
        public static bool TryParseEPS(string input, out decimal output)
        {
            try
            {
                output = Convert.ToDecimal(input.Trim());
                return true;
            }
            catch (Exception)
            {
                output = 0;
                return false;
            }
        }

        public static bool TryParseMonthsInReport(string input, out int output)
        {
            output = 0;
            try
            {
                output = Convert.ToInt32(input.Trim());
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
            try
            {
                output = Convert.ToDateTime(input.Trim());
                return output > DateTime.Now;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
