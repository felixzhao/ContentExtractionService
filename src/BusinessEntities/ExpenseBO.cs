using System;
namespace BusinessEntities
{
    public class ExpenseBO
    {
        public String CostCentre { get; set; }
        public Decimal Total { get; set; }
        public String PaymentMethod { get; set; }
        public Decimal GST { get; set; }
        public Decimal TotalExcludingGST { get; set; }
    }
}
