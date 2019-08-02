using System;

namespace BusinessEntities
{
    public class RelevantDataBO
    {
        public ExpenseBO Expense { get; set; }
        public String Vendor { get; set; }
        public String Description { get; set; }
        public DateTime Date { get; set; }
    }
}
