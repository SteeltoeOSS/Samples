using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using MySql.Data.Entity;

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

    [Table("EF6TestData")]
    public class TestData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Data { get; set; }
    }
}
