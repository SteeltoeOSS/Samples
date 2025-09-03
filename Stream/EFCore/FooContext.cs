using Microsoft.EntityFrameworkCore;

namespace EFCore;

public class FooContext : DbContext
{
    public FooContext(DbContextOptions<FooContext> options) : base(options)
    {
    }

    public DbSet<Foo> Foos { get; set; }
}