using System;
namespace ContentExtractService.Models
{
    public class RelevantData
    {
        public Expense expense { get; set; }
        public String Vendor { get; set; }
        public String Description { get; set; }
        public DateTime Date { get; set; }
        public String Email { get; set; }
    }
}
