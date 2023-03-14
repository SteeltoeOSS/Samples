using SqlServerEFCore.Entities;

namespace SqlServerEFCore.Models;

public sealed class SqlServerViewModel
{
    public string? ConnectionString { get; set; }

    public IList<SampleEntity> SampleEntities { get; set; } = new List<SampleEntity>();
}
