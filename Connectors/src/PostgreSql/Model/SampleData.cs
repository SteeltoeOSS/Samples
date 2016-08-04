using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Npgsql;

namespace PostgreSql.Models
{
    public class SampleData
    {
        internal static async Task InitializePostgresData(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            using (var service = serviceProvider.GetRequiredService<NpgsqlConnection>())
            {
                await service.OpenAsync();
                DropCreateTable(service);
                InsertSampleData(service);
                service.Close();
            }
        }

        private static void InsertSampleData(NpgsqlConnection service)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO TestData(Id, MyText) VALUES(1, 'Row1 Text');", service);
            cmd.ExecuteNonQuery();
            cmd = new NpgsqlCommand("INSERT INTO TestData(Id, MyText) VALUES(2, 'Row2 Text');", service);
            cmd.ExecuteNonQuery();
        }

        private static void DropCreateTable(NpgsqlConnection service)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("DROP TABLE IF EXISTS TestData;", service);
            cmd.ExecuteNonQuery();
            cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS TestData(Id INT PRIMARY KEY, MyText VARCHAR(255));", service);
            cmd.ExecuteNonQuery();

        }
    }
}
