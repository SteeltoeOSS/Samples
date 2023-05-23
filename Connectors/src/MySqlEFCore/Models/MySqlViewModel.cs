using MySqlEFCore.Entities;

namespace MySqlEFCore.Models;

public sealed class MySqlViewModel
{
    public string? ConnectionString { get; set; }
    public string? OtherConnectionString { get; set; }

    public IList<SampleEntity> SampleEntities { get; set; } = new List<SampleEntity>();
    public IList<OtherEntity>? OtherEntities { get; set; }
}
