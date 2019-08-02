using System;
namespace BusinessLayer
{
    public class PriceCalculator
    {
        private const decimal gstRate = 1.15M;

        public static decimal GetTotalExcludingGst(decimal total)
        {
            if(total < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(total), "Total is negative!");
            }
            return decimal.Round(decimal.Divide(total, gstRate), 2);
        }

        public static decimal GetGST(decimal total, decimal totalExcludingGst)
        {
            if (total < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(total), "Total is negative!");
            }
            return decimal.Round(total - totalExcludingGst, 2);            
        }
    }
}
