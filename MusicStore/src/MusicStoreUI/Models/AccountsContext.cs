
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MusicStoreUI.Models
{

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

}
