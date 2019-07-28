using System;
namespace BusinessLayer
{
    public class PriceCalculator
    {
        private const decimal gstRate = 1.15M;

        public static decimal GetTotalExcludingGst(decimal total)
        {
            return decimal.Round(decimal.Divide(total, gstRate), 2);
        }

        public static decimal GetGST(decimal total, decimal totalExcludingGst)
        {
            return total - totalExcludingGst;            
        }
    }
}
