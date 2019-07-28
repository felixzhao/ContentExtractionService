using System;
using BusinessEntities;

namespace BusinessLayer
{
    public class ContentExtractor : IContentExtractor
    {
        public bool GetRelevantData(string content, out RelevantDataBO relevantData)
        {
            relevantData = new RelevantDataBO()
            {
                 Expense = new ExpenseBO()
                 {
                      GST = 123M
                 }
            };
            return true;
        }
    }
}
