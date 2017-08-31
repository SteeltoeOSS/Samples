using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Steeltoe.Management.Endpoint.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStoreUI.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicStoreUI

{
    public class MySqlHealthContributor : IHealthContributor
    {
        AccountsContext _context;
        ILogger<MySqlHealthContributor> _logger;
        public MySqlHealthContributor(AccountsContext dbContext, ILogger<MySqlHealthContributor> logger)
        {
            _context = dbContext;
            _logger = logger;
        }

        public string Id { get; } = "mySql";

        public Health Health()
        {
            _logger.LogInformation("Checking MySql connection health!");

            Health result = new Health();
            result.Details.Add("database", "MySQL");
            MySqlConnection _connection = null;
            try
            {
                _connection = _context.Database.GetDbConnection() as MySqlConnection;
                if (_connection != null) {
                    _connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT 1;", _connection);
                    var qresult = cmd.ExecuteScalar();
                    result.Details.Add("result", qresult);
                    result.Details.Add("status", HealthStatus.UP.ToString());
                    result.Status = HealthStatus.UP;
                    _logger.LogInformation("MySql connection up!");
                }

            } catch (Exception e)
            {
                _logger.LogInformation("MySql connection down!");
                result.Details.Add("error", e.GetType().Name + ": " + e.Message);
                result.Details.Add("status", HealthStatus.DOWN.ToString());
                result.Status = HealthStatus.DOWN;
            } finally
            {
                if (_connection != null) _connection.Close();
            }

            return result;
        }
    }
}