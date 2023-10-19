using DocAisle_Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocAisle_Auth.Db
{
    public class UsersDbContext:IdentityDbContext<ApplicationUser>
    {
        
        public DbSet <ApplicationUser> ApplicationUsers { get; set; }
        public UsersDbContext(DbContextOptions options) : base(options)
        {


        }
        
            
        
    }
}
