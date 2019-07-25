using System;
namespace ContentExtractService.Models
{
    public class Response
    {
        Int16 StatusCode { get; set; }
        String StatusDescription { get; set; }
        String TraceId { get; set; }
    }
}
