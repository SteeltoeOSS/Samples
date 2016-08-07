
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Security.Claims;
using System.Threading.Tasks;


#if NET451 && MYSQL
using System.Data.Entity;
#endif
#if !NET451 || POSTGRES
using Microsoft.EntityFrameworkCore;
#endif

namespace MusicStoreUI.Models
{
    public static class SampleData
    {
        const string defaultAdminUserName = "DefaultAdminUsername";
        const string defaultAdminPassword = "DefaultAdminPassword";


        public static async Task InitializeAccountsDatabaseAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {

#if NET451 && MYSQL
            Database.SetInitializer<AccountsContext>(new DropCreateDatabaseAlways<AccountsContext>());
#endif
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<AccountsContext>();
#if !NET451 || POSTGRES
                await db.Database.EnsureCreatedAsync();
#endif
                await CreateAdminUser(serviceProvider, configuration);

            }
        }

        private static async Task CreateAdminUser(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            const string adminRole = "Administrator";

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new ApplicationRole(adminRole));
            }

            var user = await userManager.FindByNameAsync(configuration[defaultAdminUserName]);
            if (user == null)
            {
                user = new ApplicationUser { UserName = configuration[defaultAdminUserName] };
                await userManager.CreateAsync(user, configuration[defaultAdminPassword]);
                await userManager.AddToRoleAsync(user, adminRole);
                await userManager.AddClaimAsync(user, new Claim("ManageStore", "Allowed"));
            }

#if TESTING
            var envPerfLab = configuration["PERF_LAB"];
            if (envPerfLab == "true")
            {
                for (int i = 0; i < 100; ++i)
                {
                    var email = string.Format("User{0:D3}@example.com", i);
                    var normalUser = await userManager.FindByEmailAsync(email);
                    if (normalUser == null)
                    {
                        await userManager.CreateAsync(new ApplicationUser { UserName = email, Email = email }, "Password~!1");
                    }
                }
            }
#endif
        }

    }
}

