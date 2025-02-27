using Steeltoe.Samples.PostgreSqlEFCore.Entities;

namespace Steeltoe.Samples.PostgreSqlEFCore.Models;

public sealed class PostgreSqlViewModel
{
    public string? ConnectionString { get; set; }

    public IList<SampleEntity> SampleEntities { get; set; } = [];
}
