using Steeltoe.Samples.MySqlEFCore.Entities;

namespace Steeltoe.Samples.MySqlEFCore.Models;

public sealed class MySqlViewModel
{
    public string? ConnectionString { get; set; }
    public string? OtherConnectionString { get; set; }

    public IList<SampleEntity> SampleEntities { get; set; } = [];
    public IList<OtherEntity>? OtherEntities { get; set; }
}
