using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Steeltoe.Management.Endpoint.Health;
using System;

namespace CloudFoundry
{
    public class MySqlHealthContributor : IHealthContributor
    {
        MySqlConnection _connection;
        ILogger<MySqlConnection> _logger;
        public MySqlHealthContributor(MySqlConnection connection, ILogger<MySqlConnection> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public string Id { get; } = "mySql";

        public Health Health()
        {
            _logger.LogInformation("Checking MySql connection health!");

            Health result = new Health();
            result.Details.Add("database", "MySQL");
            try
            {
                _connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT 1;", _connection);
                var qresult = cmd.ExecuteScalar();
                result.Details.Add("result", qresult);
                result.Details.Add("status", HealthStatus.UP.ToString());
                result.Status = HealthStatus.UP;
                _logger.LogInformation("MySql connection up!");

            }
            catch (Exception e)
            {
                _logger.LogInformation("MySql connection down!");
                result.Details.Add("error", e.GetType().Name + ": " + e.Message);
                result.Details.Add("status", HealthStatus.DOWN.ToString());
                result.Status = HealthStatus.DOWN;
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }
    }
}
