using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MySqlEF6.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TestContext : DbContext
    {
        public TestContext(string connectionString) : base(connectionString)
        {

        }
        public DbSet<TestData> TestData { get; set; }
    }

    public class TestData
    {
        
        public int Id { get; set; }
        public string Data { get; set; }
    }
}
