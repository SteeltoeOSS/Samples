using Microsoft.Extensions.DependencyInjection;
using System;

namespace OrderService.Models
{
    public static class SampleData
    {
        public static void InitializeOrderDatabase(IServiceProvider serviceProvider)
        {
            if (ShouldDropCreateDatabase())
            {
                using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var db = serviceScope.ServiceProvider.GetService<OrderContext>();
                    db.Database.EnsureCreated();

                }
            }
        }

        private static bool ShouldDropCreateDatabase()
        {
            string index = Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX");
            if (string.IsNullOrEmpty(index))
            {
                return true;
            }
            int indx = -1;
            if (int.TryParse(index, out indx))
            {
                if (indx > 0) return false;
            }
            return true;
        }
    }
}

