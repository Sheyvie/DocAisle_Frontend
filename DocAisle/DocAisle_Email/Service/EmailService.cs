using DocAisle_Email.Db;
using DocAisle_Email.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DocAisle_Email.Service
{
    public class EmailService
    {
        private DbContextOptions<EmailDbContext> options;
        public EmailService()
        {
        }

        public EmailService(DbContextOptions<EmailDbContext> options)
        {
            this.options = options;
        }


        public async Task SaveToDb(EmailDto emailDto)
        {
           

            var context = new EmailDbContext(this.options);
            context.EmailDto.Add(emailDto);
            await context.SaveChangesAsync();
        }

    }
}
    

