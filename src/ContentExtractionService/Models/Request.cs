using System;
using System.ComponentModel.DataAnnotations;

namespace ContentExtractionService.Models
{
    public class Request
    {
        [Required]
        public String EmailContent { get; set; }
    }
}
