using System;
namespace ContentExtractService.Models
{
    public class Expense
    {
        String CostCentre { get; set; }
        Decimal Total { get; set; }
        String PaymentMethod { get; set; }
        Decimal GST { get; set; }
        Decimal TotalExcludingGST { get; set; }
    }
}
