using System;
namespace ContentExtractService.Models
{
    public class RelevantData
    {
        Expense expense { get; set; }
        String Vendor { get; set; }
        String Description { get; set; }
        DateTime Date { get; set; }
        String Email { get; set; }
    }
}
