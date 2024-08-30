using Steeltoe.Samples.SqlServerEFCore.Entities;

namespace Steeltoe.Samples.SqlServerEFCore.Models;

public sealed class SqlServerViewModel
{
    public string? ConnectionString { get; set; }

    public IList<SampleEntity> SampleEntities { get; set; } = new List<SampleEntity>();
}
