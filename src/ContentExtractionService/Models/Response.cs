using System;
namespace ContentExtractService.Models
{
    public class Response
    {
        public Expense Expense { get; set; }
        public String Vendor { get; set; }
        public String Description { get; set; }
        public DateTime Date { get; set; }
    }
}
