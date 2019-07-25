using System;
namespace ContentExtractService.Models
{
    public class Response
    {
        public Response()
        {
            this.TraceId = Guid.NewGuid();
        }

        public int id { get; set; }
        public Int16 StatusCode { get; set; }
        public String StatusDescription { get; set; }
        public Guid TraceId { get; set; }
        public Expense expense { get; set; }
    }
}
