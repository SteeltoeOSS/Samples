using Autofac;
using Npgsql;
using System;
using System.Data;

namespace PostgreSql4.Models
{
    public class SampleData
    {
        internal static void InitializePostgreSqlData(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Console.WriteLine("Initializing data");
            using (var connection = container.Resolve<IDbConnection>())
            {
                connection.Open();
                DropCreateTable(connection as NpgsqlConnection);
                InsertSampleData(connection as NpgsqlConnection);
                connection.Close();
                Console.WriteLine("Initializing data complete!");
            }
        }

        private static void InsertSampleData(NpgsqlConnection connection)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO TestData(Id, MyText) VALUES(1, 'Row1 Text');", connection);
            cmd.ExecuteNonQuery();
            cmd = new NpgsqlCommand("INSERT INTO TestData(Id, MyText) VALUES(2, 'Row2 Text');", connection);
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