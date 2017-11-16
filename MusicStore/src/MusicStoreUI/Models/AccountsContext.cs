using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MusicStoreUI.Models
{

    public class ApplicationUser : IdentityUser {
        
    }
    public class ApplicationRole : IdentityRole {
        public ApplicationRole() : base()
        {

        }
        public ApplicationRole(string role) : base(role)
        {
            
        }

    }

    public class AccountsContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AccountsContext(DbContextOptions<AccountsContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationRole>().Property(b => b.NormalizedName).HasMaxLength(255);
            builder.Entity<ApplicationUser>().Property(b => b.NormalizedEmail).HasMaxLength(255);
            builder.Entity<ApplicationUser>().Property(b => b.NormalizedUserName).HasMaxLength(255);
        }
    }

}
