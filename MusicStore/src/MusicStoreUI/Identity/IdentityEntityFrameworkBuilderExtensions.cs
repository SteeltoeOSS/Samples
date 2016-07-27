using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Identity;
using MusicStoreUI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MusicStoreUI.Identity
{
    public static class IdentityEntityFrameworkBuilderExtensions
    {
        public static IdentityBuilder AddEntityFrameworkStores(this IdentityBuilder builder)
        {
            builder.Services.TryAdd(GetDefaultServices());
            return builder;
        }

        private static IServiceCollection GetDefaultServices()
        {

            var services = new ServiceCollection();
            services.AddScoped(typeof(IUserStore<ApplicationUser>), typeof(UserStore));
            services.AddScoped(typeof(IRoleStore<ApplicationRole>), typeof(RoleStore));
 
            return services;
        }
    }
}
