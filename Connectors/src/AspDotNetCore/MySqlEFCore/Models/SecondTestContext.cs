using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySqlEFCore
{
    public class SecondTestContext : DbContext
    {
        public SecondTestContext(DbContextOptions<SecondTestContext> options) : base(options)
        {

        }

        public DbSet<MoreTestData> MoreTestData { get; set; }
    }

    [Table("EFCoreMoreTestData")]
    public class MoreTestData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Data { get; set; }
    }
}
