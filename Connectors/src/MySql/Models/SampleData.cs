using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace MySql.Models
{
    public class SampleData
    {
        internal static async Task InitializeMySqlData(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            using (var service = serviceProvider.GetRequiredService<MySqlConnection>())
            {
                await service.OpenAsync();
                DropCreateTable(service);
                InsertSampleData(service);
                service.Close();
            }
        }

        private static void InsertSampleData(MySqlConnection service)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO TestData(Id, MyText) VALUES(1, 'Row1 Text');", service);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("INSERT INTO TestData(Id, MyText) VALUES(2, 'Row2 Text');", service);
            cmd.ExecuteNonQuery();
        }

        private static void DropCreateTable(MySqlConnection service)
        {
            MySqlCommand cmd = new MySqlCommand("DROP TABLE IF EXISTS TestData;", service);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS TestData(Id INT PRIMARY KEY, MyText VARCHAR(255));", service);
            cmd.ExecuteNonQuery();

        }
    }
}
