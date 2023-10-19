using DocAisle_Email.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace DocAisle_Email.Db
{
    public class EmailDbContext:DbContext

    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
        {
            
        }

        public DbSet<EmailDto> EmailDto { get; set; }
    }
}
