using PostgreSqlEFCore.Entities;

namespace PostgreSqlEFCore.Models;

public sealed class PostgreSqlViewModel
{
    public string? ConnectionString { get; set; }

    public IList<SampleEntity> SampleEntities { get; set; } = new List<SampleEntity>();
}
