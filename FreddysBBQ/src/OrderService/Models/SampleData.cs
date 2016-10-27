using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OrderService.Models
{
    public static class SampleData
    {
        public static void InitializeOrderDatabase(IServiceProvider serviceProvider)
        {
            if (ShouldDropCreateDatabase())
            {
                Database.SetInitializer<OrderContext>(new DropCreateDatabaseAlways<OrderContext>());
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

