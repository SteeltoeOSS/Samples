
#if NET451 && MYSQL
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System.Data.Entity;
#endif

#if !NET451 || POSTGRES
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
#endif

using System;

namespace MusicStoreUI.Models
{

#if NET451 && MYSQL
    public class ApplicationUser : IdentityUser {
   
        public virtual string NormalizedUserName { get; set; }

        public virtual string NormalizedEmail { get; set; }

        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
    public class ApplicationRole: IdentityRole
    {
        public ApplicationRole() : base()
        {

        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public virtual string NormalizedName { get; set; }
    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class AccountsContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public AccountsContext(string connection) : base(connection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.UserName).HasMaxLength(128);
            modelBuilder.Entity<IdentityRole>().Property(r => r.Name).HasMaxLength(128);
        }
    }
#endif

#if !NET451 || POSTGRES
    public class ApplicationUser : IdentityUser { }
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
   
    }
#endif

}
