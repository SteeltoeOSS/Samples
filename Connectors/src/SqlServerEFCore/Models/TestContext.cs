using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlServerEFCore
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TestData> TestData { get; set; }
    }

    public class TestData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Data { get; set; }
    }
}
