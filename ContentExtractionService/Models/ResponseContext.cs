using System;
using ContentExtractService.Models;
using Microsoft.EntityFrameworkCore;

namespace ContentExtractionService.Models
{
    public class ResponseContext : DbContext
    {

        public ResponseContext(DbContextOptions<ResponseContext> options)
        : base(options)
        {
        }

        public DbSet<Response> Responses { get; set; }
    }
}
