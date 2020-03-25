using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MusicStoreUI.Models
{

    public class ApplicationUser : IdentityUser
    {
    }

    public class AccountsContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AccountsContext(DbContextOptions<AccountsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().Property(b => b.NormalizedName).HasMaxLength(255);
            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = "1", Name = "Administrator" });

            builder.Entity<ApplicationUser>().Property(b => b.NormalizedEmail).HasMaxLength(255);
            builder.Entity<ApplicationUser>().Property(b => b.NormalizedUserName).HasMaxLength(255);
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser { UserName = Startup.Configuration["DefaultAdminUsername"],   });
        }
    }

}
