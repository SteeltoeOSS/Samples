
using Autofac;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MySql4.Models
{
    public class SampleData
    {
        internal static void InitializeMySqlData(IContainer serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            Console.WriteLine("Initializing data");
            using (var service = serviceProvider.Resolve<IDbConnection>())
            {
                service.Open();
                DropCreateTable(service as MySqlConnection);
                InsertSampleData(service as MySqlConnection);
                service.Close();
                Console.WriteLine("Initializing data complete!");
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
