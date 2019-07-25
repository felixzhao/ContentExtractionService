using System;
namespace ContentExtractService.Models
{
    public class Expense
    {
        public int id { get; set; }
        public String CostCentre { get; set; }
        public Decimal Total { get; set; }
        public String PaymentMethod { get; set; }
        public Decimal GST { get; set; }
        public Decimal TotalExcludingGST { get; set; }
    }
}
